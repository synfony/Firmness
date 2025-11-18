namespace Firmness.Web.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DocumentId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // Used to distinguish Admin / Client
        public string PersonType { get; set; }
    }
}