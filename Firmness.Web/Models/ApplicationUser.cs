using Microsoft.AspNetCore.Identity;

namespace Firmness.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? PersonId { get; set; }   // Optional link to Person table
        public Person Person { get; set; }
    }
}

