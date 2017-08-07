using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;

namespace Hmm.Dal
{
    public class NoteCatalogStorage : StorageBase<NoteCatalog>
    {
        public NoteCatalogStorage(IUnitOfWork uow, IValidator<NoteCatalog> validator, IEntityLookup lookupRepo) : base(uow, validator, lookupRepo)
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

            UnitOfWork.Update(entity);
            return LookupRepo.GetEntity<NoteCatalog>(entity.Id);
        }

        public override bool Delete(NoteCatalog entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return false;
            }

            UnitOfWork.Delete(entity);
            return true;
        }
    }
}