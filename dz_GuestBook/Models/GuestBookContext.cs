using Microsoft.EntityFrameworkCore;

namespace dz_GuestBook.Models
{
    public class GuestBookContext : DbContext
    {
        public GuestBookContext(DbContextOptions<GuestBookContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Messages> Messages { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
