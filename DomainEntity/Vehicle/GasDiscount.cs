using DomainEntity.Misc;
using Hmm.Utility.Currency;

namespace DomainEntity.Vehicle
{
    public class GasDiscount : HmmNote
    {
        public GasDiscount()
        {
            Subject = "Discount";
        }

        public string Program { get; set; }

        public Money Amount { get; set; }
    }
}