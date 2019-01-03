using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class Specialization
    {
        public int Id { get; set; }

        public int SignupId { get; set; }
        public Signup Signup { get; set; }

        public string SpecializationName { get; set; }
        public int Role { get; set; }
    }
}
