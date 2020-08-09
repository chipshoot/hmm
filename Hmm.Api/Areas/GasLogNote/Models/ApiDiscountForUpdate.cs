using Hmm.Api.Models;
using Hmm.DomainEntity.Enumerations;

namespace Hmm.Api.Areas.GasLogNote.Models
{
    public class ApiDiscountForUpdate : ApiEntity
    {
        public string Program { get; set; }

        public float Amount { get; set; }

        public GasDiscountType DiscountType { get; set; }

        public bool IsActive { get; set; }

        public string Comment { get; set; }
    }
}