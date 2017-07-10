using DomainEntity.Misc;
using System;
using System.Collections.Generic;

namespace DomainEntity.Vehicle
{
    public class GasLog : HmmNote
    {
        public float Distance { get; set; }

        public float Gas { get; set; }

        public decimal Price { get; set; }

        public List<decimal> Discounts { get; set; }

        public string GasStation { get; set; }
    }
}