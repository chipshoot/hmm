using Hmm.Utility.Dal.DataEntity;

namespace DomainEntity.Misc
{
    public class NoteCatalog : HasDefaultEntity
    {
        public string Name { get; set; }

        public NoteRender Render { get; set; }

        public string Schema { get; set; }
    }
}