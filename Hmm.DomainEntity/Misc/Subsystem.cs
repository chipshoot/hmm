using System.Collections.Generic;
using Hmm.Utility.Dal.DataEntity;

namespace Hmm.DomainEntity.Misc
{
    public class Subsystem : HasDefaultEntity
    {
        public string Name { get; set; }

        public IEnumerable<NoteCatalog> NoteCatalogs { get; set; }
    }
}