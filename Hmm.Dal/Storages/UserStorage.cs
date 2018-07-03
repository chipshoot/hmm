using System.Collections.Generic;
using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System.Linq;

namespace Hmm.Dal.Storages
{
    public class UserStorage : StorageBase<User>
    {
        public UserStorage(
            IUnitOfWork uow,
            IValidator<User> validator,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, validator, lookupRepo, dateTimeProvider)
        {
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

            var userHasNote = LookupRepo.GetEntities<HmmNote>().Any(n => n.Author.Id == entity.Id);
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