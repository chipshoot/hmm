using System;

namespace Hmm.DtoEntity.Api.HmmNote
{
    public class ApiNote : ApiEntity
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public ApiUser Author { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}