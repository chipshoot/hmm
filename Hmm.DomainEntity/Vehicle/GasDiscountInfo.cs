using Hmm.Utility.Currency;

namespace Hmm.DomainEntity.Vehicle
{
    public class GasDiscountInfo
    {
        public GasDiscount Program { get; set; }

        public Money Amount { get; set; }
    }
}