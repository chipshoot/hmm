using System;
using Hmm.Utility.Dal;
using Hmm.Utility.Validation;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hmm.Dal.Data
{
    public class EfTransaction : IGenericTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EfTransaction(IDbContextTransaction transaction)
        {
            Guard.Against<ArgumentNullException>(transaction==null, nameof(transaction));
            _transaction = transaction;
        }
        
        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}