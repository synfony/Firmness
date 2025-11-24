using Microsoft.AspNetCore.Identity;

namespace Firmness.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
    }
}
