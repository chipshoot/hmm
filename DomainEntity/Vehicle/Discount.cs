using DomainEntity.Misc;
using Hmm.Utility.Currency;

namespace DomainEntity.Vehicle
{
    public class Discount : HmmNote
    {
        public Discount()
        {
            Subject = "Discount";
        }

        public string Program { get; set; }

        public Money Amount { get; set; }
    }
}