using DomainEntity.Misc;
using DomainEntity.Vehicle;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;

namespace Hmm.Dal.Storages
{
    public class GasLogStorage : NoteStorage
    {
        public GasLogStorage(IUnitOfWork uow,
            IValidator<HmmNote> validator,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
        }

        public override HmmNote Add(HmmNote entity)
        {
            Guard.TypeOf<GasLog>(entity, "invalid type of entity, gaslog needed");
            return base.Add(entity);
        }
    }
}