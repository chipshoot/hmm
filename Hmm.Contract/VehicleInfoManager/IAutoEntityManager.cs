using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.Misc;
using System.Collections.Generic;

namespace Hmm.Contract.VehicleInfoManager
{
    public interface IAutoEntityManager<T> where T : VehicleBase
    {
        T GetEntityById(int id);

        IEnumerable<T> GetEntities();

        T Create(T entity, User author);

        T Update(T entity, User author);

        ProcessingResult ProcessResult { get; }
    }
}