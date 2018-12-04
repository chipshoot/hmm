using Hmm.Api.Models;

namespace Hmm.Api.Areas.GasLogNote.Models
{
    public class ApiAutomobileForCreate : ApiEntity
    {
        public int MeterReading { get; set; }

        public string Brand { get; set; }

        public string Maker { get; set; }

        public string Year { get; set; }

        public string Pin { get; set; }
    }
}