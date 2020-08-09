using System.Collections.Generic;

namespace Hmm.DtoEntity.Api.GasLogNotes
{
    public class ApiGasLogForUpdate : ApiEntity
    {
        public float Distance { get; set; }

        public float Gas { get; set; }

        public decimal Price { get; set; }

        public List<ApiDiscount> Discounts { get; set; }

        public string GasStation { get; set; }
    }
}