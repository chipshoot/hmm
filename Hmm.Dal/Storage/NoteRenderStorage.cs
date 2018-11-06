using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;

namespace Hmm.Dal.Storage
{
    public class NoteRenderStorage : StorageBase<NoteRender>
    {
        public NoteRenderStorage(
            IUnitOfWork uow,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, lookupRepo, dateTimeProvider)
        {
        }

        public override NoteRender Add(NoteRender entity)
        {
            var newCat = UnitOfWork.Add(entity);
            return newCat;
        }

        public override NoteRender Update(NoteRender entity)
        {
            UnitOfWork.Update(entity);
            return LookupRepo.GetEntity<NoteRender>(entity.Id);
        }

        public override bool Delete(NoteRender entity)
        {
            UnitOfWork.Delete(entity);
            return true;
        }
    }
}