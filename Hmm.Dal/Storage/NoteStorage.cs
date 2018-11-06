using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;

namespace Hmm.Dal.Storage
{
    public class NoteStorage : StorageBase<HmmNote>
    {
        public NoteStorage(
            IUnitOfWork uow,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, lookupRepo, dateTimeProvider)
        {
        }

        public override HmmNote Add(HmmNote entity)
        {
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

        public override bool Delete(HmmNote entity)
        {
            UnitOfWork.Delete(entity);
            return true;
        }

        public override HmmNote Update(HmmNote entity)
        {
            // check if need apply default catalog
            var catalog = PropertyChecking(entity.Catalog);
            entity.Catalog = catalog ?? throw new DataSourceException("Cannot find default note catalog.");

            entity.LastModifiedDate = DateTimeProvider.UtcNow;
            UnitOfWork.Update(entity);

            var savedRec = LookupRepo.GetEntity<HmmNote>(entity.Id);

            return savedRec;
        }
    }
}