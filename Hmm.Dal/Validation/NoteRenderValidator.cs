using DomainEntity.Misc;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Validation
{
    public class NoteRenderValidator : ValidatorBase<NoteRender>
    {
        private readonly IQueryHandler<NoteRenderQueryByName, NoteRender> _queryByName;

        public NoteRenderValidator(IEntityLookup lookupRepo, IQueryHandler<NoteRenderQueryByName, NoteRender> queryByName) : base(lookupRepo)
        {
            Guard.Against<ArgumentNullException>(queryByName == null, nameof(queryByName));
            _queryByName = queryByName;
        }

        public override bool IsValid(NoteRender entity, bool isNewEntity)
        {
            // validating when try to create a new entity
            if (isNewEntity)
            {
                var savedRender = _queryByName.Execute(new NoteRenderQueryByName { RenderName = entity.Name });
                if (savedRender != null)
                {
                    ValidationErrors.Add($"The note render name {entity.Name} exists in data source");
                    return false;
                }
            }
            // validating for existing entity
            else
            {
                if (entity.Id <= 0)
                {
                    ValidationErrors.Add($"The note render does not contains valid identity {entity.Id}");
                    return false;
                }

                var savedEntity = LookupRepo.GetEntity<NoteRender>(entity.Id);
                if (savedEntity == null)
                {
                    ValidationErrors.Add($"The note render {entity.Name} does not exists in data source");
                    return false;
                }

                var exRenderSameName = _queryByName.Execute(new NoteRenderQueryByName { RenderName = entity.Name });
                if (exRenderSameName != null && exRenderSameName.Id != entity.Id)
                {
                    ValidationErrors.Add($"Duplicated render catalog name : {entity.Name} found");
                    return false;
                }
            }

            return true;
        }
    }
}