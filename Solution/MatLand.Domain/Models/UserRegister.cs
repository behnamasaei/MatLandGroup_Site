using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatLand.Domain.Models
{
    public class UserRegister
    {
        //[Required]
        //public string Name { get; set; }

        //[Required]
        //public string Family { get; set; }

        [Required]
        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string RePassword { get; set; }
    }
}
