﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class Signup
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public bool Sign { get; set; }

        public bool RoleDps { get; set; }
        public bool RoleHealer { get; set; }
        public bool RoleTank { get; set; }
    }
}
