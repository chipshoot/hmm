using Hmm.DomainEntity.Vehicle;
using System.Collections.Generic;

namespace Hmm.WebConsole.ViewModels
{
    public class GasLogIndexViewModel
    {
        public IEnumerable<GasLog> GasLogs { get; }

        public GasLogIndexViewModel(IEnumerable<GasLog> gaslogs)
        {
            GasLogs = gaslogs;
        }
    }
}