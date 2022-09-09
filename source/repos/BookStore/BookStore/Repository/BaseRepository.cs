using Microsoft.EntityFrameworkCore;
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
            throw new System.NotImplementedException();
        }

        public DbSet<T> GetTable()
        {
            return _table;
        }

        public void Insert(T obj)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> SelectAll()
        {
            return _table;
        }

        public T SelectById(object id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
