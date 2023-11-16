using Microsoft.EntityFrameworkCore;

namespace contacts.Models;

public class ContactDbContext : DbContext
{
    public ContactDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Contact> Contact { get; set; }
}