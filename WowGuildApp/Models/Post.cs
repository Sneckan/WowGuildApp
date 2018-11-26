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
    
    public List<Comment> Comments { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public enum Categories
    {
        [Description("News")]
        News = 1,
        [Description("Raid Discussion")]
        [Display(Name="Raid Discussion")]
        RaidDiscussion,
        [Description("General")]
        General,
        [Description("Classes")]
        Classes,
        [Description("Off topic")]
        OffTopic,
        [Description("Recruitment")]
        Recruitment
    }
  }
}
