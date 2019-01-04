using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WowGuildApp.Data;
using WowGuildApp.Models;

namespace WowGuildApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
          this.db = db;
        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();
            var latestPost = db.Posts.Include(p => p.User).ThenInclude(x => x.Characters).Where(p => p.Category == "News").OrderByDescending(p => p.Date).FirstOrDefault();
            if (latestPost != null)
            {
                viewModel.Character = latestPost.User.Characters.Where(c => c.Main).FirstOrDefault();
                viewModel.LatestPost = latestPost;
            }
            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
