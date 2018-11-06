using DomainEntity.User;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;

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
            try
            {
                var newUser = UnitOfWork.Add(entity);
                return newUser;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddMessage(ex.Message, true);
                return null;
            }
        }

        public override User Update(User entity)
        {
            try
            {
                UnitOfWork.Update(entity);
                var updateUser = LookupRepo.GetEntity<User>(entity.Id);
                return updateUser;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddMessage(ex.Message, true);
                return null;
            }
        }

        public override bool Delete(User entity)
        {
            try
            {
                UnitOfWork.Delete(entity);
                return true;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddMessage(ex.Message, true);
                return false;
            }
        }
    }
}