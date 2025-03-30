using Microsoft.EntityFrameworkCore;

namespace OrderProcessor.BO
{
    public class DbStorage : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbStorage(DbContextOptions<DbStorage> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
    }
}