using Hmm.Api.Models;

namespace Hmm.Api.Areas.Gaslog.Models
{
    public class ApiDiscount : ApiEntity
    {
        public string Program { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}