using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Currency;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public class GasLogManager : ManagerBase<GasLog>, IGasLogManager
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

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        public GasLog FindGasLog(int id)
        {
            var note = _noteManager.GetNoteById(id);
            if (note == null)
            {
                return null;
            }

            var gasLog = GetEntityFromNote(note);
            return gasLog;
        }

        public GasLog CreateLog(GasLog gasLog)
        {
            var catalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == "GasLog");
            if (catalog != null)
            {
                gasLog.Catalog = catalog;
            }

            SetEntityContent(gasLog);
            var note = _noteManager.Create(gasLog);

            var newGasLog = GetEntityFromNote(note);
            return newGasLog;
        }

        public GasLog CreateLogForAuthor(int authorId, GasLog gasLog)
        {
            var author = _lookupRepo.GetEntity<User>(authorId);
            if (author == null)
            {
                ProcessResult.Success = false;
                ProcessResult.AddMessage($"Cannot found author with Id {authorId}");
                return null;
            }

            gasLog.Author = author;
            var newLog = CreateLog(gasLog);
            return newLog;
        }

        protected override void SetEntityContent(GasLog gasLog)
        {
            var xml = new XElement("GasLog",
                new XElement("Automobile", gasLog.Car.Id),
                new XElement("Date", gasLog.CreateDate.ToString("O")),
                new XElement("Distance", gasLog.Distance.Measure2Xml(_noteManager.ContentNamespace)),
                new XElement("Gas", gasLog.Gas.Measure2Xml(_noteManager.ContentNamespace)),
                new XElement("Price", gasLog.Price.Measure2Xml(_noteManager.ContentNamespace)),
                new XElement("GasStation", gasLog.GasStation),
                new XElement("Discounts", "")
            );

            if (gasLog.Discounts.Any())
            {
                foreach (var disc in gasLog.Discounts)
                {
                    var discElement = new XElement("Discount",
                        new XElement("Amount", disc.Amount.Measure2Xml(_noteManager.ContentNamespace)),
                        new XElement("Program", disc.Program));
                    xml.Element("Discounts")?.Add(discElement);
                }
            }

            gasLog.Content = xml.ToString(SaveOptions.DisableFormatting);
        }

        protected override GasLog GetEntityFromNote(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var noteStr = note.Content;
            var noteXml = XDocument.Parse(noteStr);
            var ns = noteXml.Root?.GetDefaultNamespace();
            var logRoot = noteXml.Root?.Element(ns + "Content")?.Element(ns + "GasLog");
            if (logRoot == null)
            {
                return null;
            }

            var carIdStr = logRoot.Element(ns + "Automobile")?.Value;
            int.TryParse(carIdStr, out var carId);
            var car = _lookupRepo.GetEntity<Automobile>(carId);

            var gas = new GasLog
            {
                Id = note.Id,
                Author = note.Author,
                Catalog = note.Catalog,
                CreateDate = note.CreateDate,
                LastModifiedDate = note.LastModifiedDate,
                Car = car,
                Content = note.Content,
                GasStation = logRoot.Element(ns + "GasStation")?.Value,
                Description = note.Description,
                Distance = Dimension.FromXml(logRoot.Element(ns + "Distance")?.Element(ns + "Dimension")),
                Gas = Volume.FromXml(logRoot.Element(ns + "Gas")?.Element(ns + "Volume")),
                Price = Money.FromXml(logRoot.Element(ns + "Price")?.Element(ns + "Money"))
            };

            return gas;
        }
    }
}