using DomainEntity.Misc;
using Hmm.Utility.Dal.Query;
using System.Collections.Generic;

namespace Hmm.Dal.Querys
{
    public class NoteQueryByRender : IQuery<IEnumerable<HmmNote>>
    {
        public NoteRender Render { get; set; }
    }
}