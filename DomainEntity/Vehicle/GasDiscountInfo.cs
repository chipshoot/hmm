using Hmm.Utility.Currency;

namespace DomainEntity.Vehicle
{
    public class GasDiscountInfo
    {
        public GasDiscount Program { get; set; }

        public Money Amount { get; set; }
    }
}