using Hmm.Api.Models;
using System;

namespace Hmm.Api.Areas.HmmNote.Models
{
    public class ApiUserForUpdate : ApiEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }
    }
}