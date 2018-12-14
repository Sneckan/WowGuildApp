using System;
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

        public int CharacterId { get; set; }
        public Character Character{ get; set; }

        public bool Sign { get; set; }

        public int SpecializationOne { get; set; }
        public int SpecializationTwo { get; set; }
        public int SpecializationThree { get; set; }
        public int SpecializationFour { get; set; }

        public bool RoleDps { get; set; }
        public bool RoleHealer { get; set; }
        public bool RoleTank { get; set; }

        public string Note { get; set; }
    }
}
