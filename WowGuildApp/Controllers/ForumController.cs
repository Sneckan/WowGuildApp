﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WowGuildApp.Data;
using WowGuildApp.Models;

namespace WowGuildApp
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<User> _userManager;

        public ForumController(ApplicationDbContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        [Route("Forum/Categories/{category}")]
        public IActionResult Category(string category, int page = 1)
        {
            var pageSize = 5;
            var skip = pageSize * (page - 1);
            var totalOfPosts = db.Posts.Where(p => p.Category == category).Count();
            var canPage = skip < totalOfPosts;

            List<Post> postsList = new List<Post>();
            PostsViewModel viewModel = new PostsViewModel();
            viewModel.PageSize = pageSize;
            viewModel.Skip = skip;
            viewModel.MaxPage = (totalOfPosts / pageSize) + ((totalOfPosts % pageSize) > 0 ? 1 : 1);
            viewModel.Page = page;

            if (canPage)
            {
                postsList = db.Posts.Include(p => p.User).Include(p => p.Comments).Where(p => p.Category == category).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                postsList = db.Posts.Include(p => p.User).Include(p => p.Comments).Where(p => p.Category == category).OrderByDescending(p => p.Date).Take(pageSize).ToList();
            }

            viewModel.Posts = postsList;
            viewModel.Category = category;


            return View(viewModel);
        }

        // GET: Forum
        public IActionResult Index()
        {
            PostsViewModel viewModel = new PostsViewModel();
            viewModel.Categories = new List<string>();
            viewModel.LatestPosts = new List<Post>();

            foreach (var val in Enum.GetValues(typeof(Post.Categories)))
            {
                var description = EnumExtensions.GetEnumDescription((Post.Categories)val);
                viewModel.Categories.Add(description);

                var latestPost = db.Posts.Include(p => p.User).Where(p => p.Category == description).OrderByDescending(p => p.Date).FirstOrDefault();
                if (latestPost != null)
                {
                    viewModel.LatestPosts.Add(latestPost);
                }
            }

            return View(viewModel);
        }

        // GET: Forum/Details/5
        public async Task<IActionResult> Details(int? id, int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await db.Posts
                .Include(p => p.User).Include(p => p.Comments).FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var pageSize = 10;
            var skip = pageSize * (page - 1);
            var totalOfComments = db.Comments.Where(c => c.PostId == post.Id).Count();
            var canPage = skip < totalOfComments;
            PostsViewModel viewModel = new PostsViewModel();
            viewModel.Post = post;
            viewModel.Category = post.Category;
            viewModel.PageSize = pageSize;
            viewModel.Skip = skip;
            viewModel.MaxPage = (totalOfComments / pageSize) + ((totalOfComments % pageSize) > 0 ? 1 : 1);
            viewModel.Page = page;
            List<Comment> comments = new List<Comment>();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {

                viewModel.CurrentUser = user.Id;
                post.ViewCount = post.ViewCount + 1;
                db.Update(post);
                await db.SaveChangesAsync();
            }

            if (page <= 1)
            {
                viewModel.Page = 1;
                comments = db.Comments.Include(c => c.User).Where(c => c.PostId == post.Id).Take(pageSize).ToList();
            }

            else
            {
                if (canPage)
                {
                    comments = db.Comments.Include(c => c.User).Where(c => c.PostId == post.Id).Skip(skip).Take(pageSize).ToList();
                }
                else
                {
                    comments = db.Comments.Include(c => c.User).Where(c => c.PostId == post.Id).OrderByDescending(c => c.Date).Take(pageSize).OrderBy(c => c.Date).ToList();
                }
            }

            viewModel.Comments = comments;

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(PostsViewModel viewModel)
        {
            var post = db.Posts.Include(p => p.User).Include(p => p.Comments).Single(p => p.Id == viewModel.Comment.PostId);
            viewModel.Post = post;

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Comment comment = new Comment();
                comment.Text = viewModel.Text;
                comment.Date = DateTime.Now;
                comment.PostId = viewModel.Comment.PostId;
                comment.UserId = user.Id; 
                db.Add(comment);
                await db.SaveChangesAsync();
                return Redirect(viewModel.ReturnUrl + "#post" + comment.Id);
            }

            return View(viewModel);
        }

        // GET: Forum/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Category,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                post.UserId = user.Id;
                post.Date = DateTime.Now;
                db.Add(post);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Forum", new { id = post.Id });
            }
            return View(post);
        }

        [Authorize]
        // GET: Forum/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (post.UserId != user.Id)
            {
                return RedirectToAction("Details", "Forum", new { id = post.Id });
            }

            return View(post);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,Content,Date")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    post.UserId = user.Id;
                    db.Update(post);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Forum", new { id = post.Id });
            }
            return View(post);
        }

        // GET: Forum/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await db.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (post.UserId != user.Id)
            {
                return RedirectToAction("Details", "Forum", new { id = post.Id });
            }

            return View(post);
        }

        // POST: Forum/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await db.Posts.FindAsync(id);
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return db.Posts.Any(e => e.Id == id);
        }
    }
}
