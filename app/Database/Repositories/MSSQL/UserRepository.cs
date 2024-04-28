using app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Database.Repositories.MSSQL
{
    internal class UserRepository : IRepository<User>
    {
        private ApplicationContext db { get; set; }

        #region Constructor 

        public UserRepository(ApplicationContext db)
        {
            this.db = db;
        }

        #endregion

        #region Methods

        public IEnumerable<User> GetIEnumerable()
        {
            return db.Users;
        }
        public User? Get(int id)
        {
            return GetIEnumerable().ToList().Find(x => x.Id == id);
        }
        public void Create(User user)
        {
            db.Users.Add(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User? user = db.Users.Find(id);

            if (user != null)
            {
                db.Users.Remove(user);
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
