using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Utility.Misc;

namespace Hmm.Contract.VehicleInfoManager
{
    public interface IGasLogManager
    {

        GasLog GetGasLogById(int id);

        GasLog Create(GasLog gasLog, User author);

        GasLog Update(GasLog gasLog);

        ProcessingResult ProcessResult { get; }
    }
}