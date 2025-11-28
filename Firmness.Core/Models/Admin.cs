using Firmness.Core.Models;

namespace Firmness.Core.Models
{
    public class Admin : Person
    {
        public string SpecialRole { get; set; } = string.Empty;
    }
}
