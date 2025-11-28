using Microsoft.AspNetCore.Identity;
using Firmness.Core.Models;

namespace Firmness.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
    }
}
