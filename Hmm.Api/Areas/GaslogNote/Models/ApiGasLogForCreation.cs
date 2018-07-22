using System;
using System.Collections.Generic;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.GaslogNote.Models
{
    public class ApiGasLogForCreation : ApiEntity
    {
        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public float Distance { get; set; }

        public float Gas { get; set; }

        public decimal Price { get; set; }

        public List<ApiDiscountInfo> Discounts { get; set; }

        public string GasStation { get; set; }

    }
}