using System.Xml.Linq;

namespace Hmm.Utility.Misc
{
    public interface IHmmSerializable
    {
        XElement Measure2Xml(XNamespace ns);
    }
}