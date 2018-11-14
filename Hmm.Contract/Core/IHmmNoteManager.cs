using System.Collections.Generic;
using System.Xml.Linq;
using DomainEntity.Misc;
using Hmm.Utility.Misc;

namespace Hmm.Contract.Core
{
    public interface IHmmNoteManager<T> where T : HmmNote
    {
        T GetNoteById(int id);

        IEnumerable<T> GetNotes();

        T Create(T note);

        T Update(T note);

        XNamespace ContentNamespace { get; }

        ProcessingResult ProcessResult { get; }
    }
}