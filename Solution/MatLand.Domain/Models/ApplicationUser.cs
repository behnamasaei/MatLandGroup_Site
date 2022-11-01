using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MatLand.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? Family { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
        [MaxLength(20)]
        public string? StaticPhone { get; set; }

        public ApplicationUser() : base()
        {

        }
    }
}
