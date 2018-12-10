using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Signups = new List<Signup>();
        }

        public virtual List<Signup> Signups { get; set; }
    }
}
