using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal
{
    public class NoteCatalogStorage : IDataStore<NoteCatalog>
    {
        private readonly IUnitOfWork _uow;
        private readonly IQueryHandler<NoteCatalogQueryByName, NoteCatalog> _catalogQuery;

        public NoteCatalogStorage(IUnitOfWork uow, IQueryHandler<NoteCatalogQueryByName, NoteCatalog> catalogQuery)
        {
            Guard.Against<ArgumentNullException>(uow == null, nameof(uow));
            Guard.Against<ArgumentNullException>(catalogQuery == null, nameof(catalogQuery));

            _uow = uow;
            _catalogQuery = catalogQuery;
            _catalogQuery = catalogQuery;
        }

        public NoteCatalog Get(int id)
        {
            throw new NotImplementedException();
        }

        public NoteCatalog Add(NoteCatalog entity)
        {
            // find data source to check if the name is unique
            var savedCat = _catalogQuery.Execute(new NoteCatalogQueryByName { CatalogName = entity.Name });

            var newCat = savedCat == null ? _uow.Add(entity) : null;

            return newCat;
        }

        public NoteCatalog Update(NoteCatalog entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(NoteCatalog entity)
        {
            var savedCat = _catalogQuery.Execute(new NoteCatalogQueryByName { CatalogName = entity.Name });
            if (savedCat == null)
            {
                return false;
            }

            _uow.Delete(entity);
            return true;
        }

        public void Refresh(ref NoteCatalog entity)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }
    }
}