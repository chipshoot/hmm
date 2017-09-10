using DomainEntity.Vehicle;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;

namespace Hmm.Dal.Storages
{
    public class GasLogStorage : NoteStorage<GasLog>
    {
        public GasLogStorage(IUnitOfWork uow,
            IValidator<GasLog> validator,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
        }

        public override GasLog Add(GasLog entity)
        {
            Guard.TypeOf<GasLog>(entity, "invalid type of entity, gaslog needed");
            return base.Add(entity);
        }
    }
}