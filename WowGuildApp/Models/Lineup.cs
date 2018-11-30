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
        public virtual Event Event { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int Role { get; set; }
        public int Group { get; set; }

        public string OfficerNote { get; set; }
        public string Note { get; set; }
    }
}
