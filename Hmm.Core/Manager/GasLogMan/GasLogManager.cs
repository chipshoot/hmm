using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Currency;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Hmm.Core.Manager.GasLogMan
{
    public class GasLogManager : IGasLogManager
    {
        private readonly IHmmNoteManager<HmmNote> _noteManager;
        private readonly IEntityLookup _lookupRepo;

        public GasLogManager(IHmmNoteManager<HmmNote> noteManager, IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(noteManager == null, nameof(noteManager));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));
            _noteManager = noteManager;
            _lookupRepo = lookupRepo;
        }

        public ProcessingResult ErrorMessage { get; } = new ProcessingResult();

        public GasLog FindGasLog(int id)
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
            var cat = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == "GasLog");
            if (cat != null)
            {
                log.Catalog = cat;
            }

            SetGasLogContent(log);
            var note = _noteManager.Create(log);

            var newLog = GetLogFromNote(note);
            return newLog;
        }

        public GasLog CreateLogForAuthor(int authorId, GasLog log)
        {
            var author = _lookupRepo.GetEntity<User>(authorId);
            if (author == null)
            {
                ErrorMessage.Success = false;
                ErrorMessage.AddMessage($"Cannot found author with Id {authorId}");
                return null;
            }

            log.Author = author;
            var newlog = CreateLog(log);
            return newlog;
        }

        private void SetGasLogContent(GasLog gaslog)
        {
            var xml = new XElement("GasLog",
                new XElement("Date", gaslog.CreateDate.ToString("O")),
                new XElement("Distance", gaslog.Distance.Measure2Xml(_noteManager.ContentNamespace)),
                new XElement("Gas", gaslog.Gas.Measure2Xml(_noteManager.ContentNamespace)),
                new XElement("Price", gaslog.Price.Measure2Xml(_noteManager.ContentNamespace)),
                new XElement("GasStation", gaslog.GasStation),
                new XElement("Discounts", "")
            );

            if (gaslog.Discounts.Any())
            {
                foreach (var disc in gaslog.Discounts)
                {
                    var discElement = new XElement("Discount",
                        new XElement("Amount", disc.Amount.Measure2Xml(_noteManager.ContentNamespace)),
                        new XElement("Program", disc.Program));
                    xml.Element("Discounts")?.Add(discElement);
                }
            }

            gaslog.Content = xml.ToString(SaveOptions.DisableFormatting);
        }

        private GasLog GetLogFromNote(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var notestr = note.Content;
            var notexml = XDocument.Parse(notestr);
            var ns = notexml.Root?.GetDefaultNamespace();
            var logroot = notexml.Root?.Element(ns + "Content")?.Element(ns + "GasLog");
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
                GasStation = logroot.Element(ns + "GasStation")?.Value,
                Description = note.Description,
                Distance = Dimension.FromXml(logroot.Element(ns + "Distance")?.Element(ns + "Dimension")),
                Gas = Volume.FromXml(logroot.Element(ns + "Gas")?.Element(ns + "Volume")),
                Price = Money.FromXml(logroot.Element(ns + "Price")?.Element(ns + "Money"))
            };

            return gas;
        }
    }
}