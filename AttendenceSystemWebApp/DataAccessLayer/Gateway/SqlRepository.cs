using AttendenceSystemWebApp.Interfaces;
using AttendenceSystemWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AttendenceSystemWebApp.DataAccessLayer.Gateway
{
    public class SqlRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal ApplicationDbContext context;
        internal DbSet<T> dbset;

        public SqlRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbset = context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return dbset;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            var t = Find(Id);
            if (context.Entry(t).State == EntityState.Detached)
            {
                dbset.Attach(t);
            }

            dbset.Remove(t);
        }

        public T Find(int Id)
        {
            return dbset.Find(Id);
        }

        public void Insert(T t)
        {
            dbset.Add(t);
        }

        public void Update(T t)
        {
            dbset.Attach(t);
            context.Entry(t).State = EntityState.Modified;
        }
    }
}