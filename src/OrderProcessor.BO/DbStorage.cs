using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO.Entities;

namespace OrderProcessor.BO
{
    public class DbStorage : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Customers { get; set; }

        public DbStorage(DbContextOptions<DbStorage> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
    }
}