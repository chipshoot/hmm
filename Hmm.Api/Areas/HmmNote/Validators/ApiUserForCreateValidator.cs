using FluentValidation;
using Hmm.DtoEntity.Api.HmmNote;
using System;

namespace Hmm.Api.Areas.HmmNote.Validators
{
    public class ApiNoteForCreateValidator : AbstractValidator<ApiNoteForCreate>
    {
        public ApiNoteForCreateValidator()
        {
            RuleFor(note => note.Subject).NotEmpty().NotNull().Length(1, 1000);
            RuleFor(note => note.Content).NotEmpty().NotNull();
            RuleFor(note => note.Author).NotNull();
            RuleFor(note => note.CreateDate).GreaterThan(DateTime.Now);
            RuleFor(note => note.LastModifiedDate).GreaterThanOrEqualTo(note => note.CreateDate);
        }
    }
}