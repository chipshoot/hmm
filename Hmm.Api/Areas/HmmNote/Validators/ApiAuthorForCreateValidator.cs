using FluentValidation;
using Hmm.DtoEntity.Api.HmmNote;

namespace Hmm.Api.Areas.HmmNote.Validators
{
    public class ApiAuthorCreateValidator : AbstractValidator<ApiAuthorForCreate>
    {
        public ApiAuthorCreateValidator()
        {
            RuleFor(author => author.AccountNumber).NotEmpty().NotNull().Length(1, 256);
        }
    }
}