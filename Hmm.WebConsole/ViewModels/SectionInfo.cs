using System;

namespace Hmm.WebConsole.ViewModels
{
    public class SectionInfo
    {
        public int CurrentSection { get; set; }

        public int ItemsPerSection { get; set; }

        public int TotalItems { get; set; }

        public int TotalSections => (int)Math.Ceiling((decimal)TotalItems / ItemsPerSection);
    }
}