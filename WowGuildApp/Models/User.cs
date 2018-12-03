using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Data;

namespace WowGuildApp.Models
{
    public class User : IdentityUser
    {
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }

        public int? GetUserPostCount(string userId)
        {
            var postCount = Posts.Count();/*db.Posts.Where(p => p.UserId == userId).Count();*/
            //var commentCount = Comments.Count();/*db.Comments.Where(c => c.UserId == userId).Count();*/

            return postCount;
        }
    }
}
