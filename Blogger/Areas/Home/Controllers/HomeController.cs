using System.Diagnostics;
using System.Security.Claims;
using Blogger.EFCore;
using Blogger.Models;
using Dapper;
using Helpers.Constants;
using Helpers.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Areas.Home;


[Area("Home")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    /// <summary>
    /// Index page with pagination
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns></returns>
    public IActionResult Index(int page = 1, int pageSize = 2)
    {
        IActionResult returnValue = View();
        try
        {
            // Calculate skip count based on page number and page size
            int skip = (page - 1) * pageSize;

            // Get All Active Posts
            List<Post> db_posts = _dbContext.Posts
                .Where(rec => rec.Status.Equals(Status.Active))
                .OrderByDescending(rec => rec.StatusChangeDate)
                .ToList();

            // Get total number of posts
            int totalPostsCount = db_posts.Count();

            // Get paginated posts
            db_posts = db_posts
                .OrderByDescending(rec => rec.StatusChangeDate)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // Get authors for the paginated posts
            List<User> postAuthors = _dbContext.Users
                .Where(rec => rec.Status.Equals(Status.Active))
                .ToList();

            // Map posts to view models
            List<PostVM> posts = (from post in db_posts
                                  join postAuthor in postAuthors on post.AuthorId equals postAuthor.Id
                                  select new PostVM
                                  {
                                      PostId = post.Id,
                                      Title = post.Title,
                                      Content = post.PostContent,
                                      Author = $"{postAuthor.FirstName} {postAuthor.LastName}",
                                      PostTimeStamp = post.StatusChangeDate.ToString("MMMM dd, yyyy"),
                                  }).ToList();

            // Create pagination view model
            PaginationVM<PostVM> paginationViewModel = new PaginationVM<PostVM>(
                items: posts,
                pageIndex: page,
                pageSize: pageSize,
                totalItems: totalPostsCount
            );

            returnValue = View(paginationViewModel);
        }
        catch (Exception e)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            _logger.LogError(e, $"Path: {controllerName + "/" + actionName}\n" + e.Message);

            returnValue = StatusCode(500, e);
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
