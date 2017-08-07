using DomainEntity.Misc;
using Hmm.Utility.Dal.Query;

namespace Hmm.Dal.Querys
{
    public class NoteRenderQueryByName : IQuery<NoteRender>
    {
        public string RenderName { get; set; }
    }
}