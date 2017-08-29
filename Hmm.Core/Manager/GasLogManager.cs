using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Dal.DataStore;
using System.Xml;

namespace Hmm.Core.Manager
{
    public class GasLogManager : HmmNoteManager<GasLog>, IGasLogManager
    {
        public GasLogManager(IDataStore<GasLog> storage) : base(storage)
        {
        }

        public override XmlDocument GetNoteContent(GasLog note)
        {
            var xml = new XmlDocument();
            xml.LoadXml("<gaslog/>");
            var elm = xml.CreateElement("gas");
            elm.InnerXml = 

            note.Content =
                $"<gaslog><gas>{note.Gas.TotalLiter}</gas><price>{note.Price.Amount}</price><gasStation>{note.GasStation}</gasStation></gaslog>";

            return base.GetNoteContent(note);
        }
    }
}