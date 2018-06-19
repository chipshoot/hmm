using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;

namespace Hmm.Dal.Storages
{
    public class NoteStorage<T> : StorageBase<T> where T : HmmNote
    {
        public NoteStorage(
            IUnitOfWork uow,
            IValidator<T> validator,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
        }

        public override T Add(T entity)
        {
            Validator.Reset();
            if (!Validator.IsValid(entity, isNewEntity: true))
            {
                return null;
            }

            // check if need apply default catalog
            var catalog = PropertyChecking(entity.Catalog);
            entity.Catalog = catalog ?? throw new DataSourceException("Cannot find default note catalog.");

            // check if need apply default render
            var render = PropertyChecking(entity.Catalog.Render);
            entity.Catalog.Render = render ?? throw new DataSourceException("Cannot find default note render.");

            entity.CreateDate = DateTimeProvider.UtcNow;
            entity.LastModifiedDate = DateTimeProvider.UtcNow;
            var newRec = UnitOfWork.Add(entity);
            return newRec;
        }

        public override bool Delete(T entity)
        {
            Validator.Reset();
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return false;
            }

            UnitOfWork.Delete(entity);
            return true;
        }

        public override T Update(T entity)
        {
            Validator.Reset();
            if (!Validator.IsValid(entity, false))
            {
                return null;
            }

            // check if need apply default catalog
            var catalog = PropertyChecking(entity.Catalog);
            entity.Catalog = catalog ?? throw new DataSourceException("Cannot find default note catalog.");

            entity.LastModifiedDate = DateTimeProvider.UtcNow;
            UnitOfWork.Update(entity);

            var savedRec = LookupRepo.GetEntity<T>(entity.Id);

            return savedRec;
        }
    }
}