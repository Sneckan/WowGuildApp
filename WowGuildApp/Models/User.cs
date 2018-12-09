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
    }
}
