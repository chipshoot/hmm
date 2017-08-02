using System;
using Hmm.Utility.Dal;

namespace DomainEntity.Misc
{
    public class HmmNote : VersionedEntity
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public NoteCatalog Catalog { get; set; }

        public User.User Author { get; set; }

        public NoteRender Render { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}