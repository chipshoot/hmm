using System;
using FluentValidation;
using Hmm.Api.Areas.HmmNote.Models;

namespace Hmm.Api.Areas.HmmNote.Validators
{
    public class ApiUserForCreateValidator : AbstractValidator<ApiUserForCreate>
    {
        public ApiUserForCreateValidator()
        {
            RuleFor(usr => usr.FirstName).NotEmpty().NotNull().Length(1, 100);
            RuleFor(usr => usr.LastName).NotEmpty().NotNull().Length(1, 100);
            RuleFor(usr => usr.FirstName).NotEmpty().NotNull().Length(1, 100);
            RuleFor(usr => usr.BirthDay).LessThan(DateTime.Now);
            RuleFor(usr => usr.AccountName).NotEmpty().NotNull().Length(1, 256);
            RuleFor(usr => usr.Password).NotEmpty().NotNull().Length(1, 123);
        }
    }
}