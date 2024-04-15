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

namespace Blogger.Controllers;

[Area("Auth")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration? _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly ValidationService _validationService;

    public AccountController(
        ApplicationDbContext dbContext,
        ILogger<AccountController> logger,
        IConfiguration configuration,
        ValidationService validationService
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
        _validationService = validationService;
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
                var userFromDb = _dbContext.Users.Where(rec => rec.Username.Equals(model.LoginName)).FirstOrDefault();
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

                        returnValue = Redirect("/Home/Home/Index");
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

}
