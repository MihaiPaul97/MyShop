using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context;
        internal DbSet<T> dbSet;
        //DI Container figures out what needs to be injected 
        public SQLRepository(DataContext context){
            this.context=context;
            this.dbSet = context.Set<T>();
        }
        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);
            if (context.Entry(t).State == EntityState.Detached) {
                dbSet.Attach(t);
            }
            //once the object t that we find is connected to the underlined table
            //by using dbSet.Attach(t) ,  we can then remove it
            dbSet.Remove(t);
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            //Entity framework essentially caches data and DOES not immediately write it
            //to the Db, that's why we use context.SaveChanges();
            dbSet.Attach(t);//1.attach the object to the entity framework table

            //2.set that Entry to a state of modified ->this tells entity framework
            //that when SaveChanges is called, to look for the t object and persist it to
            //the database
            context.Entry(t).State = EntityState.Modified;
        }
    }
}
