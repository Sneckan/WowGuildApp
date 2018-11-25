using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
  }
}
