using DomainEntity.Misc;
using Hmm.Utility.MeasureUnit;
using System.Collections.Generic;
using Hmm.Utility.Currency;

namespace DomainEntity.Vehicle
{
    public class GasLog : HmmNote
    {
        public GasLog()
        {
            // set catalog and render for gas log
        }

        public Dimension Distance { get; set; }

        public Volume Gas { get; set; }

        public Money Price { get; set; }

        public List<Discount> Discounts { get; set; }

        public string GasStation { get; set; }
    }
}