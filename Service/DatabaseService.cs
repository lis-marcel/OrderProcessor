using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OrderProcessor.Service
{
    public class DatabaseService
    {
        private DbContext dbContext;
        public  DbContext DbContext 
        {
            set { dbContext = value; }
        }

        public DatabaseService(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add<T>(T entity) where T : class
        {
            dbContext.Set<T>().Add(entity);
            dbContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : class
        {
            dbContext.Set<T>().Update(entity);
            dbContext.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();
        }

        public T Get<T>(int id) where T : class
        {
            return dbContext.Set<T>().Find(id);
        }
    }
}
