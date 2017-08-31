using System.Xml;

namespace Hmm.Utility.Misc
{
    public interface IHmmSerializable
    {
        XmlDocument Measure2Xml();

        void Xml2Measure(XmlDocument xmlcontent);
    }
}