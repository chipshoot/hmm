using Hmm.Utility.Dal;

namespace DomainEntity.User
{
    public class User : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}