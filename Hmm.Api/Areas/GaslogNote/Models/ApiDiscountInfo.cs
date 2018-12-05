using Hmm.Api.Models;

namespace Hmm.Api.Areas.GasLogNote.Models
{
    public class ApiDiscountInfo : ApiEntity
    {
        public int DiscountId { get; set; }

        public float Amount { get; set; }
    }
}