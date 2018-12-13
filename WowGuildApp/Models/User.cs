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
        public int PostCount { get; set; }
        public User()
        {
            this.Signups = new List<Signup>();
        }

        public virtual List<Signup> Signups { get; set; }

        public List<Character> Characters { get; set; }
    }
}
