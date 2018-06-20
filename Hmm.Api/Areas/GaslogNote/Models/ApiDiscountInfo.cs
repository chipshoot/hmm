using Hmm.Api.Models;

namespace Hmm.Api.Areas.GaslogNote.Models
{
    public class ApiDiscountInfo : ApiEntity
    {
        public string Program { get; set; }

        public decimal Amount { get; set; }
    }
}