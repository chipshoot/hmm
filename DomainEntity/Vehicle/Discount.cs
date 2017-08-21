using Hmm.Utility.Currency;

namespace DomainEntity.Vehicle
{
    public class Discount
    {
        public string Program { get; set; }

        public Money Amount { get; set; }

        public string Description { get; set; }
    }
}