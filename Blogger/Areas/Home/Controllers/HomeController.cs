using System.Diagnostics;
using System.Security.Claims;
using Blogger.EFCore;
using Blogger.Models;
using Helpers.Constants;
using Helpers.ViewModels;
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

    /// <summary>
    /// Index page
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        IActionResult returnValue = View();
        try
        {
            List<Post> db_posts = _dbContext.Posts.Where(rec=>rec.Status.Equals(Status.Active)).ToList();
            List<User> postAuthors = _dbContext.Users.Where(rec => rec.Status.Equals(Status.Active)).ToList();
            List<PostVM> posts = (from post in db_posts
                                  join postAuthor in postAuthors on post.AuthorId equals postAuthor.Id
                                  select new PostVM
                                  {
                                      PostId = post.Id,
                                      Title = post.Title,
                                      Content = post.PostContent,
                                      Author = $"{postAuthor.FirstName} {postAuthor.LastName}",
                                      PostTimeStamp = post.StatusChangeDate.ToString("dd-MMM-yyyy HH:mm"),
                                  }).ToList();
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
