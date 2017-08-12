using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Dal
{
    public class UserStorage : StorageBase<User>
    {
        private readonly IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>> _noteQuery;

        public UserStorage(
            IUnitOfWork uow,
            IValidator<User> validator,
            IEntityLookup lookupRepo,
            IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>> noteQuery) : base(uow, validator, lookupRepo)
        {
            Guard.Against<ArgumentNullException>(noteQuery == null, nameof(noteQuery));

            _noteQuery = noteQuery;
        }

        public override User Add(User entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: true))
            {
                return null;
            }
            var newuser = UnitOfWork.Add(entity);
            return newuser;
        }

        public override User Update(User entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return null;
            }

            UnitOfWork.Update(entity);
            var updateuser = LookupRepo.GetEntity<User>(entity.Id);
            return updateuser;
        }

        public override bool Delete(User entity)
        {
            if (!Validator.IsValid(entity, isNewEntity: false))
            {
                return false;
            }
            var userHasNote = _noteQuery.Execute(new NoteQueryByAuthor { Author = entity }).Any();
            if (userHasNote)
            {
                Validator.ValidationErrors.Add($"Error: The user {entity.Id} still has notes in data source.");
                return false;
            }

            UnitOfWork.Delete(entity);
            return true;
        }
    }
}