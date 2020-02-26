﻿using Hmm.Utility.Dal.DataEntity;
using System;

namespace DomainEntity.User
{
    public class User : GuidEntity
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