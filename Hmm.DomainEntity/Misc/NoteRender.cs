using Hmm.Utility.Dal.DataEntity;

namespace Hmm.DomainEntity.Misc
{
    public class NoteRender : HasDefaultEntity
    {
        public string Name { get; set; }

        public string Namespace { get; set; }
    }
}