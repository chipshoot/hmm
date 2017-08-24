using DomainEntity.Misc;

namespace Hmm.Contract
{
    public interface IHmmNoteManager
    {
        HmmNote Create(HmmNote note);

        HmmNote Update(HmmNote note);
    }
}