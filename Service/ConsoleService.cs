using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Db;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace OrderProcessor.Service
{
    public class ConsoleService
    {
        private DatabaseService databaseService;
        private DbStorage dbStorage;

        public ConsoleService() 
        {
            dbStorage = new DbStorage(new DbContextOptionsBuilder<DbStorage>()
                .UseSqlite("Data Source=C:\\temp\\OrderProcessor.db").Options);
        }
    }
}
