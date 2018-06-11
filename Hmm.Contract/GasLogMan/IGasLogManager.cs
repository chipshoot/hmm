using DomainEntity.Vehicle;

namespace Hmm.Contract.GasLogMan
{
    public interface IGasLogManager : IHmmNoteManager<GasLog>
    {
        GasLog GetGasLogById(int id);

        GasLog CreateLog(GasLog log);
    }
}