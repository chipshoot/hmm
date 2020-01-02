using System.Collections.Generic;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;

namespace DomainEntity.Vehicle
{
    public class GasLog : VehicleBase
    {
        public Automobile Car { get; set; }

        public Dimension Distance { get; set; }

        public Volume Gas { get; set; }

        public Money Price { get; set; }

        public List<GasDiscountInfo> Discounts { get; set; }

        public string Station { get; set; }
    }
}