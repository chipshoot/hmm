﻿using Hmm.Api.Models;
using System;

namespace Hmm.Api.Areas.HmmNote.Models
{
    public class ApiUser : ApiEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string AccountName { get; set; }

        public bool IsActivated { get; set; }
    }
}