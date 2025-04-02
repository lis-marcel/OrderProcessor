using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service
{
    public class DbStorageService
    {
        #region Public Methods
        public static string PrepareDb()
        {
            // TODO: Find a better way to access the database
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(Path.Combine(basePath, "Db")))
            {
                Directory.CreateDirectory(Path.Combine(basePath, "Db"));
            }

            var dbPath = Path.Combine(basePath, "Db\\OrderProcessorDatabase.db");

            return dbPath;
        }

        public static int GetHighestId<T>(DbStorage dbStorageContext) where T : class
        {
            var dbSet = dbStorageContext.Set<T>();

            var highestId = dbSet.AsQueryable()
                .OrderByDescending(e => EF.Property<int>(e, "Id"))
                .Select(e => EF.Property<int>(e, "Id"))
                .FirstOrDefaultAsync().Result;

            return highestId;
        }

        #endregion

    }
}
