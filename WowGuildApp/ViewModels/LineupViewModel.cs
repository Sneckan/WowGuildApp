using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Models;

namespace WowGuildApp.ViewModels
{
    public class LineupViewModel
    {
        public Event Event { get; set; }
        public Lineup Lineup { get; set; }
        public List<Signup> Signups { get; set; }
        public List<Lineup> Lineups { get; set; }

    }
}
