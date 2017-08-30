using System.Xml;

namespace Hmm.Utility.MeasureUnit
{
    public interface IMeasureSerializable
    {
        XmlDocument Measure2Xml();

        void Xml2Measure(XmlDocument xmlcontent);
    }
}