using Hmm.Utility.MeasureUnit;
using System;
using System.Collections.Generic;
using Hmm.Utility.Currency;

namespace Hmm.DomainEntity.Vehicle
{
    public class GasLog : VehicleBase
    {
        public Automobile Car { get; set; }

        public Dimension Distance { get; set; }

        public Volume Gas { get; set; }

        public Money Price { get; set; }

        public List<GasDiscountInfo> Discounts { get; set; }

        public string Station { get; set; }

        public DateTime CreateDate { get; set; }
    }
}