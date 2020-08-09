﻿using System;
using System.Collections.Generic;

namespace Hmm.DtoEntity.Api.GasLogNotes
{
    public class ApiGasLogForCreation : ApiEntity
    {
        public DateTime CreateDate { get; set; }

        public float Distance { get; set; }

        public float Gas { get; set; }

        public decimal Price { get; set; }

        public List<ApiDiscountInfo> DiscountInfos { get; set; }

        public int AutomobileId { get; set; }

        public string GasStation { get; set; }
    }
}