using System.Collections.Generic;
using DomainEntity.Vehicle;
using Hmm.Utility.Misc;

namespace Hmm.Contract.GasLogMan
{
    public interface IAutomobileManager
    {
        IEnumerable<Automobile> GetAutomobiles();

        Automobile CreateAutomobile(Automobile car);

        Automobile UpdateAutomobile(Automobile car);

        ProcessingResult ProcessResult { get; }
    }
}