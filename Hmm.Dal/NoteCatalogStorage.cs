using System;
using System.Collections.Generic;
using System.Linq;
using DomainEntity.Misc;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;

namespace Hmm.Dal
{
    public class NoteCatalogStorage : StorageBase<NoteCatalog>
    {
        private readonly IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>> _noteQuery;

        public NoteCatalogStorage(IUnitOfWork uow, IValidator<NoteCatalog> validator, IEntityLookup lookupRepo, IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>> noteQuery) : base(uow, validator, lookupRepo)
        {
            Guard.Against<ArgumentNullException>(noteQuery == null, nameof(noteQuery));

            _noteQuery = noteQuery;
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

            // make sure there's no note attached to catalog
            var userHasNote = _noteQuery.Execute(new NoteQueryByCatalog { Catalog = entity }).Any();
            if (userHasNote)
            {
                Validator.ValidationErrors.Add($"Error: The catalog {entity.Name} still has notes in data source attached to it.");
                return false;
            }
            UnitOfWork.Delete(entity);
            return true;
        }
    }
}