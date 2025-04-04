﻿using Microsoft.EntityFrameworkCore;
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

        public static int GetHighestOrderId(DbStorage dbStorageContext)
        {
            return dbStorageContext.Orders.Any()
                ? dbStorageContext.Orders.Max(o => o.Id)
                : 0;
        }
        #endregion

    }
}
