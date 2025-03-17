using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service
{
    class DbStorageService
    {
        public static string PrepareDb()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var parentPath = Directory.GetParent(basePath).Parent.Parent.Parent.Parent.FullName;

            var dbDirectory = Path.Combine(parentPath, "OrderProcessor.BO\\Db");

            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }

            var dbPath = Path.Combine(dbDirectory, "OrderProcessor.db");

            return dbPath;
        }

        public static int GetHighestOrderId(DbStorage dbStorageContext)
        {
            return dbStorageContext.Orders.Any()
                ? dbStorageContext.Orders.Max(o => o.Id)
                : 0;
        }
    }
}
