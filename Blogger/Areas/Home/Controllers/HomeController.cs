using System.Diagnostics;
using System.Security.Claims;
using Blogger.EFCore;
using Blogger.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Areas.Home;


[Area("Home")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private ApplicationDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ApplicationDbContext context)
    {
        _logger = logger;
        _configuration = configuration;
        _dbContext = context;
    }

    public IActionResult Index()
    {
        IActionResult returnValue = View();
        try
        {
            IEnumerable<Post> posts = _dbContext.Posts;
            returnValue = View(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception at Home > Index : {ex.Message}");
            returnValue = StatusCode(500, ex);
        }
        return returnValue;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
