using DomainEntity.Vehicle;
using Hmm.Utility.Misc;

namespace Hmm.Contract.GasLogMan
{
    public interface IGasLogManager
    {
        GasLog GetGasLogById(int id);

        GasLog CreateLog(GasLog log);

        ProcessingResult ErrorMessage { get; }
    }
}