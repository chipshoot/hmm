using System.Xml;

namespace Hmm.Contract
{
    public interface INoteSerializer
    {
        XmlDocument GetSerializationXml<T>(T entity);
    }
}