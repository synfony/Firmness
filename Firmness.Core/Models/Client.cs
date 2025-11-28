using Firmness.Core.Models;
using Microsoft.AspNetCore.Identity; // Add this using statement if ApplicationUser is in Identity namespace

namespace Firmness.Core.Models
{
    public class Client : Person
    {
        public string PurchaseHistory { get; set; } = string.Empty;

        // Navigation property to link with ApplicationUser
        public string? UserId { get; set; } // Foreign key to ApplicationUser
        public ApplicationUser? User { get; set; } // Navigation property
    }
}
