namespace Firmness.Core.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string DocumentId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Used to distinguish Admin / Client
        public string PersonType { get; set; } = string.Empty;
    }
}
