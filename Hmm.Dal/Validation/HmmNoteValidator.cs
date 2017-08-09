using DomainEntity.Misc;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;

namespace Hmm.Dal.Validation
{
    public class HmmNoteValidator : ValidatorBase<HmmNote>
    {
        public HmmNoteValidator(IEntityLookup lookupRepo) : base(lookupRepo)
        {
        }

        public override bool IsValid(HmmNote entity, bool isNewEntity)
        {
            return true;
        }
    }
}