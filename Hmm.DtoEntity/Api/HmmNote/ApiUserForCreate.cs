using System;

namespace Hmm.DtoEntity.Api.HmmNote
{
    public class ApiUserForCreate : ApiEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }
    }
}