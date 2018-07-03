using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Dal.Storages
{
    public class NoteCatalogStorage : StorageBase<NoteCatalog>
    {
        public NoteCatalogStorage(
            IUnitOfWork uow,
            IValidator<NoteCatalog> validator,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
        }

        public override NoteCatalog Add(NoteCatalog entity)
        {
            // find data source to check if the name is unique
            if (!Validator.IsValid(entity, isNewEntity: true))
            {
                return null;
            }

            var newCat = UnitOfWork.Add(entity);

            return newCat;
        }

        public override NoteCatalog Update(NoteCatalog entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return null;
            }

            // check if need apply default render
            var render = PropertyChecking(entity.Render);
            entity.Render = render ?? throw new DataSourceException("Cannot find default note render.");

            UnitOfWork.Update(entity);
            return LookupRepo.GetEntity<NoteCatalog>(entity.Id);
        }

        public override bool Delete(NoteCatalog entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return false;
            }

            // make sure there's no note attached to catalog
            var catalogHasNote = LookupRepo.GetEntities<HmmNote>().Any(n => n.Catalog.Id == entity.Id);
            if (catalogHasNote)
            {
                Validator.ValidationErrors.Add($"Error: The catalog {entity.Name} still has notes in data source attached to it.");
                return false;
            }
            UnitOfWork.Delete(entity);
            return true;
        }
    }
}