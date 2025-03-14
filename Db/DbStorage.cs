using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;

namespace OrderProcessor.Db
{
    public class DbStorage : DbContext
    {
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbStorage(DbContextOptions<DbStorage> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
    }
}