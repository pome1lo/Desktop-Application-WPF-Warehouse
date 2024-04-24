using app.Database.Repositories.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Database
{
    internal class UnitOfWork : IDisposable
    {
        private ApplicationContext db = new ApplicationContext();

        private UserRepository? userRepository;
        private ProductRepository? productRepository;
        private CategoriesRepository? categoryRepository;
        private ProductFromBascketRepository? productFromBascketRepository;

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(this.db);
                }
                return userRepository;
            }
        }
        public ProductRepository Products
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(this.db);
                }
                return productRepository;
            }
        }
        
        public CategoriesRepository Categories
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new CategoriesRepository(this.db);
                }
                return categoryRepository;
            }
        }

        public ProductFromBascketRepository ProductsFromBascket
        {
            get
            {
                if (productFromBascketRepository == null)
                {
                    productFromBascketRepository = new ProductFromBascketRepository(this.db);
                }
                return productFromBascketRepository;
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
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion 
    }
}
