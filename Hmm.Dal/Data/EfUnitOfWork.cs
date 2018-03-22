using Hmm.Utility.Dal;
using Hmm.Utility.Validation;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hmm.Dal.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly IHmmDataContext _dbcontext;

        public EfUnitOfWork(IHmmDataContext dbcontext)
        {
            Guard.Against<ArgumentNullException>(dbcontext == null, nameof(dbcontext));
            _dbcontext = dbcontext;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public T Add<T>(T entity) where T : class
        {
            if (_dbcontext is DbContext context)
            {
                context.Add(entity);
                context.SaveChanges();
            }
            else
            {
                return null;
            }

            // the entity has already contains new Id
            return entity;
        }

        public void Delete<T>(T entity) where T : class
        {
            if (_dbcontext is DbContext context)
            {
            }
        }

        public void Update<T>(T entity) where T : class
        {
            throw new System.NotImplementedException();
        }

        public IGenericTransaction BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public void Flush()
        {
            throw new System.NotImplementedException();
        }
    }
}