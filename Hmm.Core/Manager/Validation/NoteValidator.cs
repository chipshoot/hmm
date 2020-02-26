using DomainEntity.Misc;
using DomainEntity.User;
using FluentValidation;
using Hmm.Dal.DataRepository;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Validation;
using System;
using System.Linq;

namespace Hmm.Core.Manager.Validation
{
    public class NoteValidator : ValidatorBase<HmmNote>
    {
        private readonly IVersionRepository<HmmNote> _dataRepo;

        public NoteValidator(IVersionRepository<HmmNote> noteRepo)
        {
            Guard.Against<ArgumentNullException>(noteRepo == null, nameof(noteRepo));
            _dataRepo = noteRepo;

            RuleFor(n => n.Subject).NotNull().Length(1, 1000);
            RuleFor(n => n.Author).NotNull().Must(AuthorNotChanged).WithMessage("Cannot update note's author");
            RuleFor(n => n.Description).Length(1, 1000);
        }

        private bool AuthorNotChanged(HmmNote note, User author)
        {
            var savedNote = _dataRepo.GetEntities().FirstOrDefault(n => n.Id == note.Id);

            // create new user, make sure account name is unique
            var authorId = author.Id;
            if (savedNote == null)
            {
                return true;
            }

            if (_dataRepo is NoteEfRepository efRepo)
            {
                return !efRepo.HasPropertyChanged(savedNote, "AuthorId");
            }
            return savedNote.Author.Id == authorId;
        }
    }
}