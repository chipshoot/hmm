using DomainEntity.User;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal
{
    public class UserStorage : StorageBase<User>
    {
        public UserStorage(IUnitOfWork uow, IValidator<User> validator, IEntityLookup lookupRepo) : base(uow, validator, lookupRepo)
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
            throw new NotImplementedException();
        }

    }
}