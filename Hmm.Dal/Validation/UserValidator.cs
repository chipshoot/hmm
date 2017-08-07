using DomainEntity.User;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Validation
{
    public class UserValidator : ValidatorBase<User>
    {
        private readonly IQueryHandler<UserQueryByAccount, User> _queryByAccountName;

        public UserValidator(IEntityLookup lookupRepo, IQueryHandler<UserQueryByAccount, User> queryByAccountName) : base(lookupRepo)
        {
            Guard.Against<ArgumentNullException>(queryByAccountName == null, nameof(queryByAccountName));

            _queryByAccountName = queryByAccountName;
        }

        public override bool IsValid(User entity, bool isNewEntity)
        {
            // validating when try to create a new entity
            if (isNewEntity)
            {
                var saveduser = _queryByAccountName.Execute(new UserQueryByAccount { AccountName = entity.AccountName });
                if (saveduser != null)
                {
                    ValidationErrors.Add($"The account name {entity.AccountName} exists in data source");
                    return false;
                }
            }
            // validating for existing entity
            else
            {
                if (entity.Id <= 0)
                {
                    ValidationErrors.Add($"The user does not contains valid identity {entity.Id}");
                    return false;
                }

                var savedEntity = LookupRepo.GetEntity<User>(entity.Id);
                if (savedEntity == null)
                {
                    ValidationErrors.Add($"The user with Id {entity.Id} does not exists in data source");
                    return false;
                }

                var exuserWithSameAccount = _queryByAccountName.Execute(new UserQueryByAccount { AccountName = entity.AccountName });
                if (exuserWithSameAccount != null && exuserWithSameAccount.Id != entity.Id)
                {
                    ValidationErrors.Add($"Duplicated account name : {entity.AccountName} found");
                    return false;
                }
            }

            return true;
        }
    }
}