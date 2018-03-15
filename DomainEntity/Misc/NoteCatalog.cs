using Hmm.Utility.Dal.DataEntity;

namespace DomainEntity.Misc
{
    public class NoteCatalog : Entity
    {
        public string Name { get; set; }

        public NoteRender Render { get; set; }

        public string Schema { get; set; }
    }
}