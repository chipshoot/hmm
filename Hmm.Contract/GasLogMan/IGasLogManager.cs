using DomainEntity.Vehicle;
using Hmm.Utility.Misc;

namespace Hmm.Contract.GasLogMan
{
    public interface IGasLogManager
    {
        GasLog FindGasLog(int id);

        GasLog CreateLog(GasLog log);

        GasLog CreateLogForAuthor(int authorId, GasLog log);

        ProcessingResult ErrorMessage { get; }
    }
}