using Hmm.DomainEntity.Enumerations;
using Hmm.Utility.Dal.DataEntity;

namespace Hmm.DomainEntity.User
{
    public class Author : GuidEntity
    {
        public string AccountName { get; set; }

        public AuthorRoleType Role { get; set; }

        public bool IsActivated { get; set; }
    }
}