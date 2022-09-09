using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace BookStore.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T SelectById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        DbSet<T> GetTable();
    }
}
