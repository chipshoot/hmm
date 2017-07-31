using Hmm.Utility.Dal;

namespace DomainEntity.Vehicle
{
    public class Automobile : VersionedEntity
    {
        public string Brand { get; set; }

        public string Maker { get; set; }

        public string Pin { get; set; }
    }
}