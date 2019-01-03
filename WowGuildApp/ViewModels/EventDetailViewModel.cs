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
        public List<Character> Characters { get; set; }
        public List<Signup> Signups { get; set; }
        public List<Specialization> SignupSpecializations { get; set; }
        public bool ConfirmedLineup { get; set; }
        public List<Lineup> Lineups{ get; set; }
        public SignupInputModel Input { get; set; }
    }

    public class SignupInputModel
    {
        public string UserId { get; set; }
        public int EventId { get; set; }
        public int CharacterId { get; set; }

        public string Note { get; set; }

        public int SpecializationOneRole { get; set; }
        public string SpecializationOneName { get; set; }
        public int SpecializationTwoRole { get; set; }
        public string SpecializationTwoName { get; set; }
        public int SpecializationThreeRole { get; set; }
        public string SpecializationThreeName { get; set; }
        public int SpecializationFourRole { get; set; }
        public string SpecializationFourName { get; set; }

    }
}
