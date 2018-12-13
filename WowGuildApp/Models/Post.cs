using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WowGuildApp.Models
{
  public class Post
  {
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public DateTime? LastEdited { get; set; }
    public int ViewCount { get; set; }
    
    public List<Comment> Comments { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public enum Categories
    {
        [Display(Name="News")]
          [Description("Guild news")]
        News = 1,
        [Display(Name="Raid Discussion")]
          [Description("Discussion concering raids")]
        RaidDiscussion,
        [Display(Name= "General")]
          [Description("General discussion relating to all things to do with World of Warcraft and the Guild")]
        General,
        [Display(Name= "Classes")]
          [Description("Everything regarding classes")]
        Classes,
        [Display(Name= "Off topic")]
          [Description("All off topic discussion in here")]
        OffTopic,
        [Display(Name= "Recruitment")]
          [Description("Guild recruitment section")]
        Recruitment
    }
  }
}
