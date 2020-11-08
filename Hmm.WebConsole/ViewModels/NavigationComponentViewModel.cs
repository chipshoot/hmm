using System.Collections.Generic;
using Hmm.DtoEntity.Api.HmmNote;

namespace Hmm.WebConsole.ViewModels
{
    public class NavigationComponentViewModel
    {
        public IEnumerable<ApiSubsystem> Subsystems { get; set; }

        public SectionInfo SubsystemsInfo { get; set; }
    }
}