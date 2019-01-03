using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class Lineup
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public int? SignupId { get; set; }
        public Signup Signup { get; set; }


        public int Role { get; set; }

        public string SpecializationName { get; set; }

        public int Group { get; set; }

        public string OfficerNote { get; set; }
        public string Note { get; set; }
    }
}
