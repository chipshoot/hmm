using DomainEntity.Misc;
using DomainEntity.Vehicle;
using Hmm.Contract;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Xml;
using System.Xml.Linq;

namespace Hmm.Core.Manager.GasLogMan
{
    public class GasLogManager : IGasLogManager
    {
        private readonly IHmmNoteManager<HmmNote> _noteManager;

        public GasLogManager(IHmmNoteManager<HmmNote> noteManager)
        {
            Guard.Against<ArgumentNullException>(noteManager == null, nameof(noteManager));
            _noteManager = noteManager;
        }

        public ProcessingResult ErrorMessage { get; } = new ProcessingResult();

        public GasLog GetGasLogById(int id)
        {
            var note = _noteManager.GetNoteById(id);
            if (note == null)
            {
                return null;
            }

            var gaslog = GetLogFromNote(note);
            return gaslog;
        }

        public GasLog CreateLog(GasLog log)
        {
            SetGasLogContent(log);
            var note = _noteManager.Create(log);

            var newLog = GetLogFromNote(note);
            return newLog;
        }

        private static void SetGasLogContent(GasLog gaslog)
        {
            var xml = new XmlDocument();
            xml.LoadXml("<GasLog/>");
            var root = xml.DocumentElement;

            var date = xml.CreateElement("Date");
            date.InnerText = gaslog.CreateDate.ToLongDateString();
            root.AppendChild(date);

            var dst = xml.CreateElement("Distance");
            dst.InnerXml = gaslog.Distance.Measure2Xml().InnerXml;
            root.AppendChild(dst);

            var gas = xml.CreateElement("Gas");
            gas.InnerXml = gaslog.Gas.Measure2Xml().InnerXml;
            root.AppendChild(gas);

            var price = xml.CreateElement("Price");
            price.InnerXml = gaslog.Price.Measure2Xml().InnerXml;
            root.AppendChild(price);

            var station = xml.CreateElement("GasStation");
            station.InnerText = gaslog.GasStation;
            root.AppendChild(station);
            gaslog.Content = xml.InnerXml;
        }

        private GasLog GetLogFromNote(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var notestr = note.Content;
            var notexml = XElement.Parse(notestr);

            return new GasLog();
        }
    }
}