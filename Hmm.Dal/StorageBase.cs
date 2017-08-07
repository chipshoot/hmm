using Hmm.Utility.Dal;
using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal
{
    public abstract class StorageBase<T> : IDataStore<T> where T : Entity
    {
        protected StorageBase(IEntityLookup lookupRepo, IUnitOfWork uow)
        {
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));
            Guard.Against<ArgumentNullException>(uow == null, nameof(uow));
            LookupRepo = lookupRepo;
            UnitOfWork = uow;
        }

        protected IEntityLookup LookupRepo { get; }

        protected IUnitOfWork UnitOfWork { get; }

        public abstract T Add(T entity);

        public abstract bool Delete(T entity);

        public abstract void Flush();

        public abstract void Refresh(ref T entity);

        public abstract T Update(T entity);
    }
}