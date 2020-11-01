using System.Collections.Generic;

namespace Hmm.WebConsole.ViewModels
{
    public class NavigationComponentViewModel
    {
        public IEnumerable<string> SubsystemTexts { get; set; }

        public string CurrentSubsystem { get; set; }

        public SectionInfo SubsystemsInfo { get; set; }
    }
}