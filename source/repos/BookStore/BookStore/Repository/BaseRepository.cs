using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace BookStore.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private ApplicationDBContext _dbContext;
        private DbSet<T> _table;

        public BaseRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }

        public void Delete(object id)
        {
            T obj = _table.Find(id);
            if (obj != null)
            {
                _table.Remove(obj);
                _dbContext.SaveChanges();
            }
        }

        public DbSet<T> GetTable()
        {
            return _table;
        }

        public void Insert(T obj)
        {
            _table.Add(obj);
            _dbContext.SaveChanges();
        }

        public IEnumerable<T> SelectAll()
        {
            return _table;
        }

        public T SelectById(object id)
        {
            return _table.Find(id);
        }

        public void Update(T obj)
        {
            _table.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
