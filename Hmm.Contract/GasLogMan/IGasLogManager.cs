using DomainEntity.Vehicle;
using Hmm.Utility.Misc;

namespace Hmm.Contract.GasLogMan
{
    public interface IGasLogManager
    {
        GasLog FindGasLog(int id);

        GasLog CreateLog(GasLog gasLog);

        GasLog CreateLogForAuthor(int authorId, GasLog gasLog);

        ProcessingResult ProcessResult { get; }
    }
}