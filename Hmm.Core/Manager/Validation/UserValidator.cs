using System;
using System.Linq;
using DomainEntity.User;
using FluentValidation;
using Hmm.Contract.Core;
using Hmm.Utility.Validation;

namespace Hmm.Core.Manager.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUserManager _userManager;

        public UserValidator(IUserManager userManager)
        {
            Guard.Against<ArgumentNullException>(userManager == null, nameof(userManager));
            _userManager = userManager;

            RuleFor(u => u.FirstName).NotNull().Length(100);
            RuleFor(u => u.LastName).NotNull().Length(100);
            RuleFor(u => u.BirthDay).NotNull();
            RuleFor(u => u.AccountName).NotNull().Length(256).Must(UniqueAccountName);
            RuleFor(u => u.Password).NotNull().Length(128);
            RuleFor(u => u.Description).Length(1000);
        }

        private bool UniqueAccountName(User user, string accountName)
        {
            var savedUser = _userManager.FindUser(user.Id);

            // create new user, make sure account name is unique
            if (savedUser == null)
            {
                var acc = accountName.Trim().ToLower();
                var sameAccountUser = _userManager.GetUsers().FirstOrDefault(u => u.AccountName.ToLower() == acc);
                if (sameAccountUser != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}