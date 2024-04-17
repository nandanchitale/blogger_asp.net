using System.Diagnostics;
using System.Security.Claims;
using Blogger.EFCore;
using Blogger.Models;
using Helpers.Constants;
using Helpers.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Helpers.Services;
using Microsoft.AspNetCore.Authorization;
using Helpers.Helper;

namespace Blogger.Controllers;

[Area("Auth")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration? _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly ValidationService _validationService;
    private readonly CommonOperations _commonOperations;

    public AccountController(
        ApplicationDbContext dbContext,
        ILogger<AccountController> logger,
        IConfiguration configuration,
        ValidationService validationService,
        CommonOperations commonOperations
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
        _validationService = validationService;
        _commonOperations = commonOperations;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        IActionResult returnValue = View();
        try
        {
            if (User.Identity.IsAuthenticated)
            {
                var accessToken = Request.Cookies["access_token"];
                returnValue = returnValue = RedirectToAction(actionName: "Index", controllerName: "Home");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            returnValue = StatusCode(500, ex);
        }
        return returnValue;
    }

    [HttpPost]
    public async Task<IActionResult> SignInAsync(SignInVM model)
    {
        IActionResult returnValue = View();
        bool isPasswordCorrect = false;
        try
        {
            if (ModelState.IsValid)
            {
                var userFromDb = _dbContext.Users.Where(rec => rec.Email.Equals(model.Email)).FirstOrDefault();
                //if the user does not exist in database
                if (userFromDb == null)
                {
                    TempData["loginPage_error"] = Messages.Login_Username_Password_Incorrect;
                    returnValue = RedirectToAction(actionName: "SignIn", controllerName: "Account");
                }
                else
                {
                    isPasswordCorrect = _validationService.ValidatePassword(model.Password, userFromDb.Password);
                    if (isPasswordCorrect)
                    {
                        await startUserSessionAsync(userFromDb);

                        returnValue = Redirect("/");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Acoount > SignIn : {ex.Message}");
            returnValue = StatusCode(500, ex);
        }
        return returnValue;
    }

    [Authorize] //only logged in user can execute this method
    public async Task<IActionResult> SignOut()
    {
        try
        {
            long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/");
        }
        catch (Exception e)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            _logger.LogError(e, $"Path: {controllerName + "/" + actionName}\n" + e.Message);
            TempData["loginPage_error"] = "There is a problem. Please Contact Administrator";
            //MenuList.Clear(); // clears the MenuList which contains all the menu items that user could have access to
            return Redirect("/");
        }
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUpAsync(SignUpVM model)
    {
        IActionResult returnValue = View();
        try
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Id = _commonOperations.GetNextSeqNumber(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = $"{model.FirstName}.{model.LastName}",
                    Password = _validationService.ConvertToHashCode(model.Password),
                    Status = Status.Active,
                    StatusChangeDate = DateTime.Now,
                };

                _dbContext.Add(user);
                _dbContext.SaveChanges();
                await startUserSessionAsync(user);

                returnValue = Redirect("/");
            }
        }
        catch (Exception e)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            _logger.LogError(e, $"Path: {controllerName + "/" + actionName}\n" + e.Message);
            TempData["loginPage_error"] = "There is a problem. Please Contact Administrator";
        }
        return returnValue;
    }

    [HttpPost]
    [Route("/Account/CheckEmail")]
    public IActionResult CheckEmailExists(string email)
    {
        IActionResult returnValue = StatusCode(500);
        try
        {
            // Check if the request is an AJAX request
            if (Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                // Check if the email exists
                bool emailExists = _validationService.isEmailExists(email);

                // Return response based on email existence
                returnValue = Json(new { exists = emailExists });
            }
        }
        catch (Exception e)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            _logger.LogError(e, $"Path: {controllerName + "/" + actionName}\n" + e.Message);
            TempData["signuppage_error"] = "There is a problem. Please Contact Administrator";
        }
        return returnValue;
    }

    /// <summary>
    /// This method is used to setup claims of user and start user session
    /// Either from sign in or from sign up
    /// </summary>
    private async Task startUserSessionAsync(User userFromDb)
    {
        try
        {
            #region [SIGN IN, COOKIE BASED AUTHENTICATION]
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                            new Claim(ClaimTypes.Name, userFromDb.FirstName + " " + userFromDb.LastName),
                            new Claim("login_name", userFromDb.Username),
                            new Claim("status",userFromDb.Status),
                            new Claim("status_change_date",userFromDb.StatusChangeDate.ToString()),
                        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = false,
            });
            #endregion
        }
        catch(Exception e)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            _logger.LogError(e, $"Path: {controllerName + "/" + actionName}\n" + e.Message);
            TempData["signuppage_error"] = "There is a problem. Please Contact Administrator";
        }
    }
}
