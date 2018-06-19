using DomainEntity.Misc;
using DomainEntity.Vehicle;
using Hmm.Contract;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Xml.Linq;
using Hmm.Utility.Currency;

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
            var xml = new XElement("GasLog",
                new XElement("Date", gaslog.CreateDate.ToString("O")),
                new XElement("Distance", gaslog.Distance.Measure2Xml()),
                new XElement("Gas", gaslog.Gas.Measure2Xml()),
                new XElement("Price", gaslog.Price.Measure2Xml()),
                new XElement("GasStation", gaslog.GasStation),
                new XElement("Discounts", "")
            );

            gaslog.Content = xml.ToString();
        }

        private GasLog GetLogFromNote(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var notestr = note.Content;
            var notexml = XDocument.Parse(notestr);
            var logroot = notexml.Root?.Element("Content")?.Element("GasLog");
            if (logroot == null)
            {
                return null;
            }

            var gas = new GasLog
            {
                Id = note.Id,
                Author = note.Author,
                Catalog = note.Catalog,
                CreateDate = note.CreateDate,
                LastModifiedDate = note.LastModifiedDate,
                Content = note.Content,
                GasStation = logroot.Element("GasStation")?.Value,
                Description = note.Description,
                Distance = Dimension.FromXml(logroot.Element("Distance")),
                Gas = Volume.FromXml(logroot.Element("Gas")),
                Price = Money.FromXml(logroot.Element("Price"))
            };

            return gas;
        }
    }
}