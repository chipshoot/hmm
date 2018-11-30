using DomainEntity.Misc;
using FluentValidation;
using Hmm.Utility.Validation;

namespace Hmm.Core.Manager.Validation
{
    public class NoteCatalogValidator : ValidatorBase<NoteCatalog>
    {
        public NoteCatalogValidator()
        {
            RuleFor(c => c.Name).NotNull().Length(1, 200);
            RuleFor(c => c.Schema).NotNull();
            RuleFor(c => c.Render).NotNull();
            RuleFor(c => c.IsDefault).NotNull();
            RuleFor(c => c.Description).Length(1, 1000);
        }
    }
}