using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WowGuildApp.Data;

namespace WowGuildApp.Controllers
{
    public class ForumController : Controller
    {

        private ApplicationDbContext db;

        public ForumController(ApplicationDbContext db)
        {
          this.db = db;
        }

        public enum Categories
        {
            [Display(Name = "News")]
            News = 1,
            [Display(Name = "Raid Discussion")]
            RaidDiscussion,
            [Display(Name = "General")]
            General,
            [Display(Name = "Classes")]
            Classes,
            [Display(Name = "Off topic")]
            OffTopic,
            [Display(Name = "Recruitment")]
            Recruitment
        }

        public IActionResult Index()
        {
          return View();
        }

        [Route("Forum/{category}")]
        public IActionResult Category(string category)
        {
           var list = db.Posts.Where(p => p.Category == category);
           return View(list);
        }
    }
}