using Hmm.DomainEntity.Enumerations;

namespace Hmm.DtoEntity.Api.GasLogNotes
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