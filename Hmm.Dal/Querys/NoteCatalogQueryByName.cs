using DomainEntity.Misc;
using Hmm.Utility.Dal.Query;

namespace Hmm.Dal.Querys
{
    public class NoteCatalogQueryByName : IQuery<NoteCatalog>
    {
        public string CatalogName { get; set; }
    }
}