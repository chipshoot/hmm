using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using Hmm.Dal.Querys;
using Hmm.Utility.Misc;

namespace Hmm.Dal
{
    public class NoteRenderStorage : StorageBase<NoteRender>
    {
        private readonly IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>> _noteQuery;

        public NoteRenderStorage(
            IUnitOfWork uow,
            IValidator<NoteRender> validator,
            IEntityLookup lookupRepo,
            IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>> noteQuery,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
            Guard.Against<ArgumentNullException>(noteQuery == null, nameof(noteQuery));
            _noteQuery = noteQuery;
        }

        public override NoteRender Add(NoteRender entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: true))
            {
                return null;
            }

            var newCat = UnitOfWork.Add(entity);
            return newCat;
        }

        public override NoteRender Update(NoteRender entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return null;
            }

            UnitOfWork.Update(entity);
            return LookupRepo.GetEntity<NoteRender>(entity.Id);
        }

        public override bool Delete(NoteRender entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return false;
            }

            // make sure there's no note attached to the render
            var renderHasNote = _noteQuery.Execute(new NoteQueryByRender { Render = entity }).Any();
            if (renderHasNote)
            {
                Validator.ValidationErrors.Add($"Error: The render {entity.Name} still has notes in data source attached to it.");
                return false;
            }

            UnitOfWork.Delete(entity);
            return true;
        }
    }
}