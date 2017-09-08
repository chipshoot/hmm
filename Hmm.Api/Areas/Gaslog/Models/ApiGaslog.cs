using System.Collections.Generic;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.Gaslog.Models
{
    public class ApiGaslog : ApiEntity
    {
        public float Distance { get; set; }

        public float Gas { get; set; }

        public decimal Price { get; set; }

        public List<ApiDiscount> Discounts { get; set; }

        public string GasStation { get; set; }
    }
}