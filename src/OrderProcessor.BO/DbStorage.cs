using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OrderProcessor.BO
{
    public class DbStorage : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbStorage(DbContextOptions<DbStorage> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
    }
}