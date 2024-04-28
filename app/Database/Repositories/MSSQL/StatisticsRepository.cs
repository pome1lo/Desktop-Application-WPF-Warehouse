using app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Database.Repositories.MSSQL
{
    internal class StatisticsRepository : IRepository<Statistics>
    {
        private ApplicationContext db { get; set; }

        #region Constructor 

        public StatisticsRepository(ApplicationContext db)
        {
            this.db = db;
        }

        #endregion

        #region Methods

        public void Create(Statistics item)
        {
            db.StatisticalData.Add(item);
        }

        public void Delete(int id)
        {
            Statistics? type = db.StatisticalData.Find(id);

            if (type != null)
            {
                db.StatisticalData.Remove(type);
            }
        } 

        public Statistics? Get(int id)
        {
            return db.StatisticalData.Find(id);
        }

        public IEnumerable<Statistics> GetIEnumerable()
        {
            return db.StatisticalData;
        }

        public void Update(Statistics item)
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
