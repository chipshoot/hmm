using System.Xml;
using Hmm.Utility.Misc;

namespace Hmm.Contract.Core
{
    public interface INoteSerializer
    {
        XmlDocument GetSerializationXml<T>(T entity);

        ProcessingResult ErrorMessage { get; }
    }
}