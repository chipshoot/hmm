using Hmm.Utility.Dal;
using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Validation;
using System;
using Hmm.Utility.Dal.Query;

namespace Hmm.Dal
{
    public abstract class StorageBase<T> : IDataStore<T> where T : Entity
    {
        protected StorageBase(IUnitOfWork uow, IValidator<T> validator, IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(uow == null, nameof(uow));
            Guard.Against<ArgumentNullException>(validator == null, nameof(validator));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));
            UnitOfWork = uow;
            Validator = validator;
            LookupRepo = lookupRepo;
        }

        public IValidator<T> Validator { get; set; }

        protected IEntityLookup LookupRepo { get; }

        protected IUnitOfWork UnitOfWork { get; }

        public abstract T Add(T entity);

        public abstract bool Delete(T entity);

        public abstract T Update(T entity);
    }
}