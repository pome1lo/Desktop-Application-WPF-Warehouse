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

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository();
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
                    productRepository = new ProductRepository();
                }
                return productRepository;
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
