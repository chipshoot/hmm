using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Utility.Misc;
using System.Linq;

namespace Hmm.Contract.VehicleInfoManager
{
    public interface IAutoEntityManager<T> where T : VehicleBase
    {
        T GetEntityById(int id);

        IQueryable<T> GetEntities();

        T Create(T entity, User author);

        T Update(T entity, User author);

        ProcessingResult ProcessResult { get; }
    }
}