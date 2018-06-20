using Hmm.Utility.Currency;

namespace DomainEntity.Vehicle
{
    public class GasDiscountInfo
    {
        public string Program { get; set; }

        public Money Amount { get; set; }
    }
}