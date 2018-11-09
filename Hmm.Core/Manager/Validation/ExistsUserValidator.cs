using DomainEntity.User;
using Hmm.Utility.Dal.Query;

namespace Hmm.Dal.Validation
{
    public class ExistsUserValidator : ExistsElementValidator<User>
    {
        public ExistsUserValidator(IEntityLookup lookupRepo) : base(lookupRepo)
        {
            Include(new UserValidator2());
        }
    }
}