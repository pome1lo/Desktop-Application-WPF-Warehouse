using app.Database;
using app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Database.Repositories.MSSQL
{
    internal class ProductFromBascketRepository : IRepository<ProductFromBasket>
    {
        private ApplicationContext db { get; set; }

        #region Constructor 

        public ProductFromBascketRepository(ApplicationContext db)
        {
            this.db = db;
        }

        #endregion

        #region Methods

        public IEnumerable<ProductFromBasket> GetIEnumerable()
        {
            return db.ProductsFromBasket
                .Include(x => x.Product)
                .Include(x => x.Product.Category);
        }

        public ProductFromBasket? Get(int product)
        {
            return db.ProductsFromBasket.Include(x => x.Product).FirstOrDefault(x=> x.Id == product);
        }

        public void Create(ProductFromBasket item)
        {
            db.ProductsFromBasket.Add(item);
        }

        public void Update(ProductFromBasket item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ProductFromBasket? productFromBasket = db.ProductsFromBasket.Find(id);

            if (productFromBasket != null)
            {
                db.ProductsFromBasket.Remove(productFromBasket);
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
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
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
