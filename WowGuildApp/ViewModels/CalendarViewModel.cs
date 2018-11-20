using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.ViewModels
{
    public class CalendarViewModel
    {
        public DateTime date { get; set; }
        public string next { get; set; }
        public string prev { get; set; }
    }
}
