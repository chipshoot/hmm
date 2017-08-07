using DomainEntity.User;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;

namespace Hmm.Dal
{
    public class UserStorage : StorageBase<User>
    {
        public UserStorage(IEntityLookup lookupRepo, IUnitOfWork uow) : base(lookupRepo, uow)
        {
        }

        public override User Add(User entity)
        {
            throw new System.NotImplementedException();
        }

        public override User Update(User entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool Delete(User entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Refresh(ref User entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Flush()
        {
            throw new System.NotImplementedException();
        }
    }
}