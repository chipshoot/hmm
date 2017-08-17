using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;

namespace Hmm.Dal
{
    public class NoteStorage : StorageBase<HmmNote>
    {
        private const int DefaultPropId = 1;

        public NoteStorage(
            IUnitOfWork uow,
            IValidator<HmmNote> validator,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
        }

        public override HmmNote Add(HmmNote entity)
        {
            Validator.Reset();
            if (!Validator.IsValid(entity, isNewEntity: true))
            {
                return null;
            }

            // check if need apply default catalog
            var catalog = PropertyChecking(entity.Catalog);
            entity.Catalog = catalog ?? throw new DataSourceException("Cannot find default note catalog.");

            // check if need apply default catalog
            var render = PropertyChecking(entity.Render);
            entity.Render = render ?? throw new DataSourceException("Cannot find default note render.");

            entity.CreateDate = DateTimeProvider.UtcNow;
            entity.LastModifiedDate = DateTimeProvider.UtcNow;
            var newRec = UnitOfWork.Add(entity);
            return newRec;
        }

        public override bool Delete(HmmNote entity)
        {
            Validator.Reset();
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return false;
            }

            UnitOfWork.Delete(entity);
            return true;
        }

        public override HmmNote Update(HmmNote entity)
        {
            Validator.Reset();
            if (!Validator.IsValid(entity, false))
            {
                return null;
            }

            // check if need apply default catalog
            var catalog = PropertyChecking(entity.Catalog);
            entity.Catalog = catalog ?? throw new DataSourceException("Cannot find default note catalog.");

            // check if need apply default render
            var render = PropertyChecking(entity.Render);
            entity.Render = render ?? throw new DataSourceException("Cannot find default note render.");

            entity.LastModifiedDate = DateTimeProvider.UtcNow;
            UnitOfWork.Update(entity);

            var savedRec = LookupRepo.GetEntity<HmmNote>(entity.Id);

            return savedRec;
        }

        private T PropertyChecking<T>(T property) where T : Entity
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
            else if (LookupRepo.GetEntity<T>(property.Id) == null)
            {
                defaultNeeded = true;
            }

            if (!defaultNeeded)
            {
                return property;
            }

            var defaultProp = LookupRepo.GetEntity<T>(DefaultPropId);
            return defaultProp;
        }
    }
}