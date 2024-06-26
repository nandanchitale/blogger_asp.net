﻿using System;
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
using Microsoft.Extensions.Hosting;

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
        public IActionResult Index()
        {
            IActionResult returnValue = View();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                IQueryable<Post> posts = _context.Posts.Where(rec => rec.AuthorId == session_user && rec.Status.Equals(Status.Active)).OrderByDescending(rec => rec.StatusChangeDate).AsQueryable();
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
                    Post db_post = _context.Posts.Where(rec => rec.Id == id && rec.Status.Equals(Status.Active)).FirstOrDefault();
                    if (db_post is not null)
                    {
                        List<User> users = _context.Users.Where(rec => rec.Status.Equals(Status.Active)).ToList();
                        User postAuthor = users.Where(rec => rec.Id == db_post.AuthorId).FirstOrDefault();
                        List<PostComment> postComments = _context.PostComments.Where(rec => rec.PostId == id).OrderByDescending(rec => rec.StatusChangeDate).ToList();

                        PostVM post = new PostVM
                        {
                            PostId = db_post.Id,
                            Title = db_post.Title,
                            Content = db_post.PostContent,
                            Author = $"{postAuthor.FirstName} {postAuthor.LastName}",
                            PostTimeStamp = db_post.StatusChangeDate.ToString("MMMM dd, yyyy"),

                            PostComments = (
                                from postComment in postComments
                                join user in users on postComment.UserId equals user.Id
                                select new PostCommentsVM
                                {
                                    PostCommentId = postComment.Id,
                                    PostID = db_post.Id,
                                    Commenter = $"{user.FirstName} {user.LastName}",
                                    CommentText = postComment.CommentText,
                                    PostCommentTimeStamp = postComment.StatusChangeDate.ToString("MMMM dd, yyyy"),
                                }
                            ).ToList()
                        };

                        returnValue = View(post);
                    }
                }
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

        // GET: Posts/Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
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

                    returnValue = Redirect("/home");
                }
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

        // GET: Posts/Posts/Edit/5
        [Authorize]
        public IActionResult Edit(long? id)
        {
            IActionResult returnValue = NotFound();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                if (id is not null)
                {
                    var db_post = _context.Posts.Where(rec => rec.Id == id && rec.AuthorId == session_user).FirstOrDefault();
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
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _logger.LogError(e, $"Path: {controllerName + "/" + actionName}\n" + e.Message);

                returnValue = StatusCode(500, e);
            }
            return returnValue;
        }

        // POST: Posts/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
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
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _logger.LogError(e, $"Path: {controllerName + "/" + actionName}\n" + e.Message);

                returnValue = StatusCode(500, e);
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
        public IActionResult Delete(long? id)
        {
            IActionResult returnValue = NotFound();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                if (id is not null)
                {
                    Post db_post = _context.Posts.Where(rec => rec.Id == id && rec.AuthorId == session_user).FirstOrDefault();
                    User postAuthor = _context.Users.Where(rec => rec.Id == db_post.AuthorId).FirstOrDefault();

                    if (db_post is not null && postAuthor is not null)
                    {
                        PostVM post = new PostVM
                        {
                            PostId = db_post.Id,
                            Title = db_post.Title,
                            Content = db_post.PostContent,
                            Author = $"{postAuthor.FirstName} {postAuthor.LastName}",
                            PostTimeStamp = db_post.StatusChangeDate.ToString("MMMM dd, yyyy, HH:mm"),
                        };
                        returnValue = View(post);
                    }

                }
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
                if (post is not null)
                {
                    post.Status = Status.Inactive;
                    post.StatusChangeDate = DateTime.Now;
                    _context.SaveChanges();
                    returnValue = RedirectToAction(nameof(Index));
                }
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

        // POST: Posts/Posts/Delete/5
        [HttpPost, ActionName("AddComment")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddComment(PostCommentsVM postCommentsVM)
        {
            IActionResult returnValue = NotFound();
            try
            {
                long session_user = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                if (ModelState.IsValid)
                {
                    PostComment postComment = new PostComment
                    {
                        PostId = postCommentsVM.PostID,
                        UserId = session_user,
                        CommentText = postCommentsVM.CommentText,
                        Status = Status.Active,
                        StatusChangeDate = DateTime.Now
                    };

                    _context.Add(postComment);
                    _context.SaveChanges();

                    returnValue = Redirect("/");
                }
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
    }
}
