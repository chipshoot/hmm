using DomainEntity.Vehicle;
using Hmm.Utility.Misc;
using System.Linq;
using DomainEntity.User;

namespace Hmm.Contract.VehicleInfoManager
{
    public interface IAutomobileManager
    {
        Automobile GetAutomobileById(int id);

        IQueryable<Automobile> GetAutomobiles();

        Automobile Create(Automobile car, User author);

        Automobile Update(Automobile car, User author);

        ProcessingResult ProcessResult { get; }
    }
}