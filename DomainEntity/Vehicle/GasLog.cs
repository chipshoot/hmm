using DomainEntity.Misc;
using Hmm.Utility.MeasureUnit;
using System.Collections.Generic;

namespace DomainEntity.Vehicle
{
    public class GasLog : HmmNote
    {
        public Dimension Distance { get; set; }

        public Volume Gas { get; set; }

        public decimal Price { get; set; }

        public List<Discount> Discounts { get; set; }

        public string GasStation { get; set; }
    }
}