using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Data;

namespace WowGuildApp.Models
{
    public class PostsViewModel
    {
        public Post Post { get; set; }
        public List<Post> Posts { get; set; }
        public List<Post> LatestPosts { get; set; }
        public string Category { get; set; }
        public List<string> Categories { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment Comment { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int MaxPage { get; set; }
        public string ReturnUrl { get; set; }
        public string CurrentUser { get; set; }

        //[StringLength(300, MinimumLength = 3, ErrorMessage = "Text must be atleast 3 charcters long and cannot exceed 300 characters.")]
        public string Text { get; set; }
    }
}
