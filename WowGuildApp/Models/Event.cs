using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string InviteTime { get; set; }
        public string StartTime { get; set; }
        public string LastSignup { get; set; }

        public virtual string hostId { get; set; }
        public virtual User host { get; set; }
    }
}
