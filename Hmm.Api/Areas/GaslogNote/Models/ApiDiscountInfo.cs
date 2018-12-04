using DomainEntity.Enumerations;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.GasLogNote.Models
{
    public class ApiDiscountInfo : ApiEntity
    {
        public int Id { get; set; }

        public string Program { get; set; }

        public GasDiscountType DiscountType { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }
    }
}