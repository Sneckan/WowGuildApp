﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WowGuildApp.Data;
using WowGuildApp.Models;

namespace WowGuildApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController()
        {
          this.db = db;
        }

        public IActionResult Index()
        {
            var post = new Post();
            post.Date = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();
            return View();
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
