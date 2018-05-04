﻿using DomainEntity.Misc;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;
using System.Collections.Generic;

namespace DomainEntity.Vehicle
{
    public class GasLog : HmmNote
    {
        public GasLog()
        {
            // set catalog and render for gas log
            Subject = "GasLog";
        }

        public Dimension Distance { get; set; }

        public Volume Gas { get; set; }

        public Money Price { get; set; }

        public List<GasDiscount> Discounts { get; set; }

        public string GasStation { get; set; }
    }
}