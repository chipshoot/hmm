using DomainEntity.User;
using Hmm.Utility.Dal.Query;

namespace Hmm.Dal.Querys
{
    public class UserQueryByAccount : IQuery<User>
    {
        public string AccountName { get; set; }
    }
}