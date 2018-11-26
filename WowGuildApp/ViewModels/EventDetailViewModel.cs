using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Models;

namespace WowGuildApp.ViewModels
{
    public class EventDetailViewModel
    {
        public Event Event { get; set; }
        public User Host { get; set; }
        public Signup Signup { get; set; }
        public List<Signup> Signups { get; set; }
    }
}
