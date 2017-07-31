using System;
using Hmm.Utility.Dal;

namespace DomainEntity.Misc
{
    public class HmmNote : VersionedEntity
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public int Catalog { get; set; }

        public int Author { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public byte[] Version { get; set; }
    }
}