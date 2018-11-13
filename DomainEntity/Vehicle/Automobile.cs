using DomainEntity.Misc;

namespace DomainEntity.Vehicle
{
    public class Automobile : HmmNote
    {
        public Automobile()
        {
            Subject = "Automobile";
        }

        public int MeterReading { get; set; }

        public string Brand { get; set; }

        public string Maker { get; set; }

        public string Year { get; set; }

        public string Pin { get; set; }
    }
}