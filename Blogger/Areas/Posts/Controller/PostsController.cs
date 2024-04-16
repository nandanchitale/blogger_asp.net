using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blogger.Models;
using Blogger.EFCore;
using Microsoft.AspNetCore.Authorization;
using Helpers.ViewModels;
using Helpers.Constants;
using System.Security.Claims;

namespace Blogger.Areas.Posts
{
    [Area("Posts")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PostsController> _logger;

        public PostsController(ApplicationDbContext context, ILogger<PostsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Posts/Posts
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            IActionResult returnValue = null;
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                IQueryable<Post> posts = _context.Posts.Where(rec => rec.AuthorId == session_user).AsQueryable();
                returnValue = View(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Posts > Create : {ex.Message}");
                returnValue = StatusCode(500, ex.Message);
            }
            return returnValue;
        }

        // GET: Posts/Posts/Details/5
        /// <summary>
        /// Method to Get the Post Data from ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Details(long? id)
        {
            IActionResult returnValue = NotFound();
            try
            {
                if (id is not null)
                {
                    Post db_post = _context.Posts.Where(rec => rec.Id == id).FirstOrDefault();
                    User postAuthor = _context.Users.Where(rec => rec.Id == db_post.AuthorId).FirstOrDefault();

                    if (db_post is not null && postAuthor is not null)
                    {
                        PostVM post = new PostVM
                        {
                            PostId = db_post.Id,
                            Title = db_post.Title,
                            Content = db_post.PostContent,
                            Author = $"{postAuthor.FirstName} {postAuthor.LastName}",
                            PostTimeStamp = db_post.StatusChangeDate.ToString("MMMM dd, yyyy"),
                        };
                        returnValue = View(post);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Posts > Create : {ex.Message}");
                returnValue = StatusCode(500, ex.Message);
            }
            return returnValue;
        }

        // GET: Posts/Posts/Create
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostVM postVM)
        {
            IActionResult returnValue = View();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                if (ModelState.IsValid)
                {
                    Post post = new Post()
                    {
                        Title = postVM.Title,
                        PostContent = postVM.Content,
                        AuthorId = session_user,
                        Status = Status.Active,
                        StatusChangeDate = DateTime.Now,
                    };

                    _context.Posts.Add(post);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Posts > Create : {ex.Message}");
                returnValue = StatusCode(500, ex.Message);
            }
            return returnValue;
        }

        // GET: Posts/Posts/Edit/5
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Set<User>(), "Id", "FirstName", post.AuthorId);
            return View(post);
        }

        // POST: Posts/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PostVM PostVM)
        {
            IActionResult returnValue = NotFound();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                if (ModelState.IsValid)
                {

                    Post post = _context.Posts.Where(rec => rec.Id == PostVM.PostId && rec.AuthorId == session_user).FirstOrDefault();
                    if (post is not null)
                    {
                        post.Title = PostVM.Title;
                        post.PostContent = PostVM.Content;
                        post.StatusChangeDate = DateTime.Now;
                    }
                    _context.SaveChanges();

                    returnValue = RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Posts > Create : {ex.Message}");
                returnValue = StatusCode(500, ex.Message);
            }
            return returnValue;
        }

        // GET: Posts/Posts/Delete/5
        /// <summary>
        /// Method to Show post before deletion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long? id)
        {
            IActionResult returnValue = NotFound();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                if (id is not null)
                {
                    Post db_post = _context.Posts.Where(rec => rec.Id == id && rec.AuthorId == session_user).FirstOrDefault();
                    User postAuthor = _context.Users.Where(rec => rec.Id == db_post.Id).FirstOrDefault();

                    if (db_post is not null && postAuthor is not null)
                    {
                        PostVM post = new PostVM
                        {
                            PostId = db_post.Id,
                            Title = db_post.Title,
                            Content = db_post.PostContent,
                            Author = $"{postAuthor.FirstName} {postAuthor.LastName}",
                            PostTimeStamp = db_post.StatusChangeDate.ToString("MMMM dd, yyyy"),
                        };
                        returnValue = View(post);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Posts > Create : {ex.Message}");
                returnValue = StatusCode(500, ex.Message);
            }
            return returnValue;
        }

        // POST: Posts/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteConfirmed(long id)
        {
            IActionResult returnValue = NotFound();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                Post post = _context.Posts.Where(rec => rec.Id == id && rec.AuthorId == session_user).FirstOrDefault();
                post.Status = Status.Inactive;
                post.StatusChangeDate = DateTime.Now;
                _context.SaveChanges();
                returnValue = RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Posts > Create : {ex.Message}");
                returnValue = StatusCode(500, ex.Message);
            }
            return returnValue;
        }
    }
}
