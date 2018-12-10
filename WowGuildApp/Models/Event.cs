using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class Event
    {
        public Event()
        {
            this.Signups = new List<Signup>();
        }

        public int Id { get; set; }


        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime InviteTime { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime LastSignup { get; set; }

        public virtual string hostId { get; set; }
        public virtual User host { get; set; }

        public virtual  List<Signup> Signups { get; set; }
        public bool ConfirmedLineup { get; set; }
        public virtual List<Lineup> Lineup { get; set; }

    }
}
