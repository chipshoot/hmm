using DomainEntity.User;
using FluentValidation;

namespace Hmm.Dal.Validation
{
    public class UserValidator2 : AbstractValidator<User>
    {
        public UserValidator2()
        {
            RuleFor(u => u.FirstName).NotNull().Length(100);
            RuleFor(u => u.LastName).NotNull().Length(100);
            RuleFor(u => u.BirthDay).NotNull();
            RuleFor(u => u.AccountName).NotNull().Length(256);
            RuleFor(u => u.Password).NotNull().Length(128);
            RuleFor(u => u.Description).Length(1000);
        }
    }
}