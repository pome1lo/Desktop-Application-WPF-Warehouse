﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Database
{
    interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetIEnumerable();
        T? Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
