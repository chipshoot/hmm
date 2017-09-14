using System;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.HmmNote.Models
{
    public class ApiUser : ApiEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }

        public bool IsActivated { get; set; }
    }
}