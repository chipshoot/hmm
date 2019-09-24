using DomainEntity.User;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Storage
{
    public class UserStorage : StorageBase<User>
    {
        public UserStorage(
            IUnitOfWork uow,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, lookupRepo, dateTimeProvider)
        {
        }

        public override User Add(User entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                var newUser = UnitOfWork.Add(entity);
                return newUser;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public override User Update(User entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // ReSharper disable once PossibleNullReferenceException
                if (entity.Id <= 0)
                {
                    ProcessMessage.Success = false;
                    ProcessMessage.AddErrorMessage($"Can not update user with id {entity.Id}");
                    return null;
                }

                UnitOfWork.Update(entity);
                var updateUser = LookupRepo.GetEntity<User>(entity.Id);
                return updateUser;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public override bool Delete(User entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                UnitOfWork.Delete(entity);
                return true;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return false;
            }
        }
    }
}