using DomainEntity.User;
using FluentValidation;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Validation;
using System;
using System.Linq;

namespace Hmm.Core.Manager.Validation
{
    public class UserValidator : ValidatorBase<User>
    {
        private readonly IDataStore<User> _dataSource;

        public UserValidator(IDataStore<User> userSource)
        {
            Guard.Against<ArgumentNullException>(userSource == null, nameof(userSource));
            _dataSource = userSource;

            RuleFor(u => u.FirstName).NotNull().Length(1, 100);
            RuleFor(u => u.LastName).NotNull().Length(1, 100);
            RuleFor(u => u.BirthDay).NotNull().Must(ValidBirthday).WithMessage("Invalid birthday");
            RuleFor(u => u.AccountName).NotNull().Length(1, 256).Must(UniqueAccountName).WithMessage("Duplicated account name");
            RuleFor(u => u.Password).NotNull().Length(1, 128);
            RuleFor(u => u.Description).Length(1, 1000);
        }

        private static bool ValidBirthday(DateTime birthday)
        {
            var result = birthday != DateTime.MaxValue && 
                   birthday != DateTime.MinValue && 
                   birthday <= DateTime.Now &&
                   birthday.Date != DateTime.Now.Date;
            return result;
        }

        private bool UniqueAccountName(User user, string accountName)
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