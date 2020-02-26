using System.Collections.Generic;
using System.Xml.Linq;
using DomainEntity.Misc;
using Hmm.Utility.Misc;

namespace Hmm.Contract.Core
{
    public interface IHmmNoteManager
    {
        HmmNote GetNoteById(int id);

        IEnumerable<HmmNote> GetNotes();

        HmmNote Create(HmmNote note);

        HmmNote Update(HmmNote note);

        XNamespace ContentNamespace { get; }

        ProcessingResult ProcessResult { get; }
    }
}