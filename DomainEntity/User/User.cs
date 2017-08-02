using System;
using Hmm.Utility.Dal;

namespace DomainEntity.User
{
    public class User : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public bool IsActivated { get; set; }
    }
}