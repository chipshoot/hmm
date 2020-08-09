using Hmm.DomainEntity.Enumerations;

namespace Hmm.DtoEntity.Api.GasLogNotes
{
    public class ApiDiscount : ApiEntity
    {
        public int Id { get; set; }

        public string Program { get; set; }

        public GasDiscountType DiscountType { get; set; }

        public decimal Amount { get; set; }

        public int AuthorId { get; set; }

        public bool IsActive { get; set; }
    }
}