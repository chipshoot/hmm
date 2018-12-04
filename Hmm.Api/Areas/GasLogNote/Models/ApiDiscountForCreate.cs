using DomainEntity.Enumerations;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.GasLogNote.Models
{
    public class ApiDiscountForCreate : ApiEntity
    {
        public string Program { get; set; }

        public float Amount { get; set; }

        public GasDiscountType DiscountType { get; set; }

        public bool IsActive { get; set; }

        public string Comment { get; set; }
    }
}