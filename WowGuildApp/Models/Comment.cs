using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
  public class Comment
  {
    [Key]
    public int Id { get; set; }
    [MinLength(3,ErrorMessage = "Text must be atleast 3 characters long.")]
    [MaxLength(300,ErrorMessage="Text cannot be greater than 300 characters.")]
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }
  }
}
