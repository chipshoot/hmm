using FluentValidation;
using Hmm.DomainEntity.User;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Validation;
using System;
using System.Linq;

namespace Hmm.Core.Manager.Validation
{
    public class AuthorValidator : ValidatorBase<Author>
    {
        private readonly IGuidRepository<Author> _dataSource;

        public AuthorValidator(IGuidRepository<Author> userSource)
        {
            Guard.Against<ArgumentNullException>(userSource == null, nameof(userSource));
            _dataSource = userSource;

            RuleFor(u => u.AccountName).NotNull().Length(1, 256).Must(UniqueAccountName).WithMessage("Duplicated account name");
            RuleFor(u => u.Description).Length(1, 1000);
        }

        private bool UniqueAccountName(Author user, string accountName)
        {
            var savedUser = _dataSource.GetEntities().FirstOrDefault(e => e.Id == user.Id);

            // create new user, make sure account name is unique
            var acc = accountName.Trim().ToLower();
            if (savedUser == null)
            {
                var sameAccountUser = _dataSource.GetEntities().FirstOrDefault(u => u.AccountName.ToLower() == acc);
                if (sameAccountUser != null)
                {
                    return false;
                }
            }
            else
            {
                var userWithAccount = _dataSource.GetEntities()
                    .FirstOrDefault(u => u.AccountName.ToLower() == acc && u.Id != user.Id);

                return userWithAccount == null;
            }

            return true;
        }
    }
}