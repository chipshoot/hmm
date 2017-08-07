using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;

namespace Hmm.Dal
{
    public class NoteRenderStorage : StorageBase<NoteRender>
    {
        public NoteRenderStorage(IUnitOfWork uow, IValidator<NoteRender> validator, IEntityLookup lookupRepo) : base(uow, validator, lookupRepo)
        {
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

            UnitOfWork.Delete(entity);
            return true;
        }
    }
}