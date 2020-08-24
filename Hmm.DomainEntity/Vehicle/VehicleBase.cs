using System;

namespace Hmm.DomainEntity.Vehicle
{
    public class VehicleBase
    {
        public int Id { get; set; }

        public Guid AuthorId { get; set; }
    }
}