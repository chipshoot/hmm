using DomainEntity.Misc;

namespace DomainEntity.Vehicle
{
    public class Automobile : Entity
    {
        public string Brand { get; set; }

        public string Maker { get; set; }

        public string Pin { get; set; }
    }
}