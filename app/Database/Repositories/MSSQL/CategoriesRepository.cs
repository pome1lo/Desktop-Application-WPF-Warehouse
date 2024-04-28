using app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace app.Database.Repositories.MSSQL
{
    internal class CategoriesRepository : IRepository<Category>
    {
        private ApplicationContext db { get; set; }

        #region Constructor 

        public CategoriesRepository(ApplicationContext db)
        {
            this.db = db;
        }

        #endregion

        #region Methods

        public void Create(Category item)
        {
            db.Categories.Add(item);
        }

        public void Delete(int id)
        {
            Category? type = db.Categories.Find(id);

            if (type != null)
            {
                db.Categories.Remove(type);
            }
        }

        public Category GetByName(string Name)
        {
            if (!db.Categories.ToList().Any(x => x.Name == Name))
            {
                db.Categories.Add(new Category() { Name = Name });
                db.SaveChanges();
            }
            return db.Categories.First(x => x.Name == Name);
        }

        public Category? Get(int id)
        {
            return db.Categories.Find(id);
        }

        public IEnumerable<Category> GetIEnumerable()
        {
            return db.Categories;
        }

        public void Update(Category item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        #region Dispose

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #endregion
    }
}
