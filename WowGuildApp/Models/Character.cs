using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class Character
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public string Realm { get; set; }
        public string Battlegroup { get; set; }
        public int Class { get; set; }
        public int Race { get; set; }
        public int Gender { get; set; }
        public int Level { get; set; }
        public long Achievmentpoints { get; set; }
        public string Thumbnail { get; set; }
        public long LastModified { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
