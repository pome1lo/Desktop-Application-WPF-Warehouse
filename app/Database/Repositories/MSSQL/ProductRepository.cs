using app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Database.Repositories.MSSQL
{
    internal class ProductRepository : IRepository<Product>
    {
        private ApplicationContext db { get; set; }

        #region Constructor 

        public ProductRepository(ApplicationContext db)
        {
            this.db = db;
        }

        #endregion

        #region Methods

        public IEnumerable<Product> GetIEnumerable()
        {
            //IEnumerable<Product> products = db.Products;

            //products.ToList().ForEach(x => x.Description = db.Descriptions.ToArray()[x.Id - 1]);

            return db.Products.Include(x => x.Category);
        }

        public void Update(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
        }

        public Product? Get(int id)
        {
            return db.Products.Find(id);
        }

        public void Create(Product product)
        {
            //product.Id = db.Products.Last().Id + 1;
            db.Products.Add(product);
        }

        public void Delete(int id)
        {
            Product? product = db.Products.Find(id);

            if (product != null)
            {
                db.Products.Remove(product);
            }
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
