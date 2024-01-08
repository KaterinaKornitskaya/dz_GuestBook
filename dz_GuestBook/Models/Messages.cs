namespace dz_GuestBook.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime MessageDate { get; set; }
        public User? User { get; set; }
    }
}
