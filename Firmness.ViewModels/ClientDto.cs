namespace Firmness.ViewModels
{
    public class ClientDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string DocumentId { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
