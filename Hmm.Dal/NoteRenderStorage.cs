using DomainEntity.Misc;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal
{
    public class NoteRenderStorage : StorageBase<NoteRender>
    {
        private readonly IQueryHandler<NoteRenderQueryByName, NoteRender> _renderQuery;

        public NoteRenderStorage(IEntityLookup lookupRepo, IUnitOfWork uow, IQueryHandler<NoteRenderQueryByName, NoteRender> renderQuery) : base(lookupRepo, uow)
        {
            Guard.Against<ArgumentNullException>(renderQuery == null, nameof(renderQuery));

            _renderQuery = renderQuery;
        }

        public override NoteRender Add(NoteRender entity)
        {
            // find data source to check if the name is unique
            var savedCat = _renderQuery.Execute(new NoteRenderQueryByName { RenderName = entity.Name });

            var newCat = savedCat == null ? UnitOfWork.Add(entity) : null;

            return newCat;
        }

        public override NoteRender Update(NoteRender entity)
        {
            var savedRec = LookupRepo.GetEntity<NoteRender>(entity.Id);
            if (savedRec == null)
            {
                return null;
            }

            // make sure the note render name is unique in database
            var sameNameRender = _renderQuery.Execute(new NoteRenderQueryByName { RenderName = entity.Name });
            if (sameNameRender != null && sameNameRender.Id != entity.Id)
            {
                return null;
            }

            UnitOfWork.Update(entity);
            return LookupRepo.GetEntity<NoteRender>(entity.Id);
        }

        public override bool Delete(NoteRender entity)
        {
            var savedRender = LookupRepo.GetEntity<NoteRender>(entity.Id);
            if (savedRender == null)
            {
                return false;
            }

            UnitOfWork.Delete(entity);
            return true;
        }

        public override void Refresh(ref NoteRender entity)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }
    }
}