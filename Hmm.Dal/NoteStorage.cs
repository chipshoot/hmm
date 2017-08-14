using DomainEntity.Misc;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;

namespace Hmm.Dal
{
    public class NoteStorage : StorageBase<HmmNote>
    {
        public NoteStorage(
            IUnitOfWork uow,
            IValidator<HmmNote> validator,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
        }

        public override HmmNote Add(HmmNote entity)
        {
            Validator.Reset();
            if (!Validator.IsValid(entity, isNewEntity: true))
            {
                return null;
            }

            entity.CreateDate = DateTimeProvider.UtcNow;
            entity.LastModifiedDate = DateTimeProvider.UtcNow;
            var newRec = UnitOfWork.Add(entity);
            return newRec;
        }

        public override bool Delete(HmmNote entity)
        {
            throw new System.NotImplementedException();
        }

        public override HmmNote Update(HmmNote entity)
        {
            throw new System.NotImplementedException();
        }
    }
}