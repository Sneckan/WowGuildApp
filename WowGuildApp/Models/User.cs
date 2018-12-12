﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class User : IdentityUser
    {

        public List<Character> Characters { get; set; }
    }
}
