using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.RequestObject
{
    public class LoginRequestObject
    {
        [Required, MinLength(3), MaxLength(15)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
