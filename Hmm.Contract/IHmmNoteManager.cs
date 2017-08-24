using DomainEntity.Misc;
using System.Xml;

namespace Hmm.Contract
{
    public interface IHmmNoteManager<T> where T : HmmNote
    {
        T Create(T note);

        T Update(T note);

        XmlDocument GetNoteContent(T note);
    }
}