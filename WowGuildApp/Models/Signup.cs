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

        public List<Specialization> Specializations { get; set; }

        public bool Sign { get; set; }
        

        public string Note { get; set; }
    }
}
