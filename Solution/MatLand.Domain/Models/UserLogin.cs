using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatLand.Domain.Models
{
    public class UserLogin
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password{ get; set; }
    }
}
