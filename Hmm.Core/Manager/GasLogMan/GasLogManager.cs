using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using System.Xml;

namespace Hmm.Core.Manager.GasLogMan
{
    public class GasLogManager : HmmNoteManager<GasLog>, IGasLogManager
    {
        public GasLogManager(IDataStore<GasLog> storage, IEntityLookup lookup) : base(storage, lookup)
        {
        }

        protected override XmlDocument GetNoteContent(GasLog note, bool isXmlContent = false)
        {
            var xml = new XmlDocument();
            xml.LoadXml("<gaslog/>");
            var root = xml.DocumentElement;

            var gas = xml.CreateElement("gas");
            gas.InnerXml = note.Gas.Measure2Xml().InnerXml;
            root.AppendChild(gas);

            var dst = xml.CreateElement("distance");
            dst.InnerXml = note.Distance.Measure2Xml().InnerXml;
            root.AppendChild(dst);

            var station = xml.CreateElement("gasStation");
            station.InnerText = note.GasStation;
            root.AppendChild(station);

            note.Content = xml.InnerXml;

            return base.GetNoteContent(note, true);
        }
    }
}