using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;

namespace Hmm.Dal.Storage
{
    public class NoteCatalogStorage : StorageBase<NoteCatalog>
    {
        public NoteCatalogStorage(
            IUnitOfWork uow,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, lookupRepo, dateTimeProvider)
        {
        }

        public override NoteCatalog Add(NoteCatalog entity)
        {
            var newCat = UnitOfWork.Add(entity);

            return newCat;
        }

        public override NoteCatalog Update(NoteCatalog entity)
        {
            // check if need apply default render
            var render = PropertyChecking(entity.Render);
            entity.Render = render ?? throw new DataSourceException("Cannot find default note render.");

            UnitOfWork.Update(entity);
            return LookupRepo.GetEntity<NoteCatalog>(entity.Id);
        }

        public override bool Delete(NoteCatalog entity)
        {
            UnitOfWork.Delete(entity);
            return true;
        }
    }
}