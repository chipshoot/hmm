using Hmm.Utility.Dal;
using Hmm.Utility.Validation;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hmm.Dal.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly IHmmDataContext _dbContext;

        public EfUnitOfWork(IHmmDataContext dbContext)
        {
            Guard.Against<ArgumentNullException>(dbContext == null, nameof(dbContext));
            _dbContext = dbContext;
        }

        public void Dispose()
        {
        }

        public T Add<T>(T entity) where T : class
        {
            if (_dbContext is DbContext context)
            {
                try
                {
                    context.Add(entity);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new DataSourceException(ex.Message, ex);
                }
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
            if (_dbContext is DbContext context)
            {
                try
                {
                    context.Remove(entity);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new DataSourceException(ex.Message, ex);
                }
            }
        }

        public void Update<T>(T entity) where T : class
        {
            if (_dbContext is DbContext context)
            {
                try
                {
                    context.Update(entity);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new DataSourceException(ex.Message, ex);
                }
            }
        }

        public IGenericTransaction BeginTransaction()
        {
            if (!(_dbContext is DbContext context))
            {
                return null;
            }

            var transaction = new EfTransaction(context.Database.BeginTransaction());
            return transaction;
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }
    }
}