using Hmm.Utility.Dal;
using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Dal.Storage
{
    public abstract class StorageBase<T> : IDataStore<T> where T : Entity
    {
        protected StorageBase(IUnitOfWork uow, IEntityLookup lookupRepo, IDateTimeProvider dateTimeProvider)
        {
            Guard.Against<ArgumentNullException>(uow == null, nameof(uow));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));
            Guard.Against<ArgumentNullException>(dateTimeProvider == null, nameof(dateTimeProvider));

            UnitOfWork = uow;
            LookupRepo = lookupRepo;
            DateTimeProvider = dateTimeProvider;
        }

        protected IEntityLookup LookupRepo { get; }

        protected IDateTimeProvider DateTimeProvider { get; }

        protected IUnitOfWork UnitOfWork { get; }

        public abstract T Add(T entity);

        public abstract bool Delete(T entity);

        public abstract T Update(T entity);

        public IEnumerable<T> GetEntities()
        {
            return LookupRepo.GetEntities<T>();
        }

        public ProcessingResult ProcessMessage { get; } = new ProcessingResult();

        protected TP PropertyChecking<TP>(TP property) where TP : HasDefaultEntity
        {
            var defaultNeeded = false;
            if (property == null)
            {
                defaultNeeded = true;
            }
            else if (property.Id <= 0)
            {
                defaultNeeded = true;
            }
            else if (LookupRepo.GetEntity<TP>(property.Id) == null)
            {
                defaultNeeded = true;
            }

            if (!defaultNeeded)
            {
                return property;
            }

            var defaultProp = LookupRepo.GetEntities<TP>().FirstOrDefault(p => p.IsDefault);
            return defaultProp;
        }
    }
}