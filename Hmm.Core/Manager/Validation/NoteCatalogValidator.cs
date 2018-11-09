using DomainEntity.Misc;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Validation
{
    public class NoteCatalogValidator : ValidatorBase<NoteCatalog>
    {
        private readonly IQueryHandler<NoteCatalogQueryByName, NoteCatalog> _queryByName;

        public NoteCatalogValidator(IEntityLookup lookupRepo, IQueryHandler<NoteCatalogQueryByName, NoteCatalog> queryByName) : base(lookupRepo)
        {
            Guard.Against<ArgumentNullException>(queryByName == null, nameof(queryByName));
            _queryByName = queryByName;
        }

        public override bool IsValid(NoteCatalog entity, bool isNewEntity)
        {
            // validating when try to create a new entity
            if (isNewEntity)
            {
                var savedCat = _queryByName.Execute(new NoteCatalogQueryByName { CatalogName = entity.Name });
                if (savedCat != null)
                {
                    ValidationErrors.Add($"The note catalog name {entity.Name} exists in data source");
                    return false;
                }
            }
            // validating for existing entity
            else
            {
                if (entity.Id <= 0)
                {
                    ValidationErrors.Add($"The note catalog does not contains valid identity {entity.Id}");
                    return false;
                }

                var savedEntity = LookupRepo.GetEntity<NoteCatalog>(entity.Id);
                if (savedEntity == null)
                {
                    ValidationErrors.Add($"The note catalog {entity.Name} does not exists in data source");
                    return false;
                }

                var existsCatWithSameName = _queryByName.Execute(new NoteCatalogQueryByName { CatalogName = entity.Name });
                if (existsCatWithSameName != null && existsCatWithSameName.Id != entity.Id)
                {
                    ValidationErrors.Add($"Duplicated note catalog name : {entity.Name} found");
                    return false;
                }
            }

            return true;
        }
    }
}