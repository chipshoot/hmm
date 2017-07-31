using System;
using System.Linq;
using System.Linq.Expressions;
using Hmm.Utility.Dal;

namespace Hmm.Dal
{
    public class DefaultRepository<T> : IRepository<T> where T : Entity
    {
        public IQueryable<T> FindEntities()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> FindEntities(Expression<Func<T, bool>> query)
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Refresh(ref T entity)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public T FindEntityById(int id)
        {
            throw new NotImplementedException();
        }
    }
}