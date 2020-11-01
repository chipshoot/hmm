using Hmm.Utility.Dal.DataEntity;

namespace Hmm.DomainEntity.Misc
{
    public class NoteCatalog : HasDefaultEntity
    {
        public string Name { get; set; }

        public Subsystem Subsystem { get; set; }

        public NoteRender Render { get; set; }

        public string Schema { get; set; }
    }
}