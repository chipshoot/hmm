using DomainEntity.Misc;
using Hmm.Utility.Dal.Query;
using System.Collections.Generic;

namespace Hmm.Dal.Querys
{
    public class NoteQueryByCatalog : IQuery<IEnumerable<HmmNote>>
    {
        public NoteCatalog Catalog { get; set; }
    }
}