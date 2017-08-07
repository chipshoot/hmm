using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;
using Hmm.Dal.Querys;

namespace Hmm.Dal
{
    public class NoteCatalogStorage : StorageBase<NoteCatalog>
    {
        private readonly IQueryHandler<NoteCatalogQueryByName, NoteCatalog> _catalogQuery;

        public NoteCatalogStorage(IEntityLookup lookupRepo, IUnitOfWork uow, IQueryHandler<NoteCatalogQueryByName, NoteCatalog> catalogQuery) : base(lookupRepo, uow)
        {
            Guard.Against<ArgumentNullException>(catalogQuery == null, nameof(catalogQuery));

            _catalogQuery = catalogQuery;
        }

        public override NoteCatalog Add(NoteCatalog entity)
        {
            // find data source to check if the name is unique
            var savedCat = _catalogQuery.Execute(new NoteCatalogQueryByName { CatalogName = entity.Name });

            var newCat = savedCat == null ? UnitOfWork.Add(entity) : null;

            return newCat;
        }

        public override NoteCatalog Update(NoteCatalog entity)
        {
            var savedRec = LookupRepo.GetEntity<NoteCatalog>(entity.Id);
            if (savedRec == null)
            {
                return null;
            }

            // make sure the note catalog name is unique in database
            var sameNameCat = _catalogQuery.Execute(new NoteCatalogQueryByName {CatalogName = entity.Name});
            if (sameNameCat != null && sameNameCat.Id != entity.Id)
            {
                return null;
            }

            UnitOfWork.Update(entity);
            return LookupRepo.GetEntity<NoteCatalog>(entity.Id);
        }

        public override bool Delete(NoteCatalog entity)
        {
            var savedCat = LookupRepo.GetEntity<NoteCatalog>(entity.Id);
            if (savedCat == null)
            {
                return false;
            }

            UnitOfWork.Delete(entity);
            return true;
        }

        public override void Refresh(ref NoteCatalog entity)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }
    }
}