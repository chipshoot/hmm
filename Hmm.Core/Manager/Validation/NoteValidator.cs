using DomainEntity.Misc;
using DomainEntity.User;
using FluentValidation;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Validation;
using System;
using System.Linq;

namespace Hmm.Core.Manager.Validation
{
    public class NoteValidator : ValidatorBase<HmmNote>
    {
        private readonly IDataStore<HmmNote> _dataSource;

        public NoteValidator(IDataStore<HmmNote> noteSource)
        {
            Guard.Against<ArgumentNullException>(noteSource == null, nameof(noteSource));
            _dataSource = noteSource;

            RuleFor(n => n.Subject).NotNull().Length(1, 1000);
            RuleFor(n => n.Author).NotNull().Must(AuthorNotChanged).WithMessage("Cannot update note's author");
            RuleFor(n => n.Description).Length(1, 1000);
        }

        private bool AuthorNotChanged(HmmNote note, User author)
        {
            var savedNote = _dataSource.GetEntities().FirstOrDefault(n => n.Id == note.Id);

            // create new user, make sure account name is unique
            var authorId = author.Id;
            if (savedNote == null)
            {
                return true;
            }

            return savedNote.Author.Id == authorId;
        }
    }
}