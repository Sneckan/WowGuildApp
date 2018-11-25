using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Data;

namespace WowGuildApp.Models
{
    public class PostsViewModel
    {
        public List<Post> Posts { get; set; }
        public List<Post> LatestPosts { get; set; }
        public string Category { get; set; }
        public List<string> Categories { get; set; }
    }
}
