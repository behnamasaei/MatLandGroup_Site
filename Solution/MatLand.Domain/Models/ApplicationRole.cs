using Microsoft.AspNetCore.Identity;

namespace MatLand.Domain.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {

        }
    }
}
