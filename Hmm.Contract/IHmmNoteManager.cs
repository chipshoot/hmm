using DomainEntity.Misc;
using Hmm.Utility.Misc;

namespace Hmm.Contract
{
    public interface IHmmNoteManager<T> where T : HmmNote
    {
        T GetNoteById(int id);

        T Create(T note);

        T Update(T note);

        ProcessingResult ErrorMessage { get; }
    }
}