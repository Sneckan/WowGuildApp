using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WowGuildApp.Data;

namespace WowGuildApp.Controllers
{
    public class Forum2Controller : Controller
    {

        private ApplicationDbContext db;

        public Forum2Controller(ApplicationDbContext db)
        {
          this.db = db;
        }

        public enum Categories
        {
            [Description("News")]
            News = 1,
            [Description("Raid Discussion")]
            RaidDiscussion,
            [Description("General")]
            General,
            [Description("Classes")]
            Classes,
            [Description("Off topic")]
            OffTopic,
            [Description("Recruitment")]
            Recruitment
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        public IActionResult Index()
        {
          return View(db.Posts.ToList());
        }

        [Route("Forum/{category}")]
        public IActionResult Category(string category)
        {
           var list = db.Posts.Where(p => p.Category == category);
           return View(list);
        }
    }
}