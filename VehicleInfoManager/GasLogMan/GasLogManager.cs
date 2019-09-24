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
        private readonly IAutomobileManager _carManager;
        private readonly IDiscountManager _discountManager;
        private readonly IEntityLookup _lookupRepo;

        public GasLogManager(
            IHmmNoteManager<HmmNote> noteManager,
            IAutomobileManager carManager,
            IDiscountManager discountManager,
            IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(noteManager == null, nameof(noteManager));
            Guard.Against<ArgumentNullException>(carManager == null, nameof(carManager));
            Guard.Against<ArgumentNullException>(discountManager == null, nameof(discountManager));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));

            _noteManager = noteManager;
            _carManager = carManager;
            _discountManager = discountManager;
            _lookupRepo = lookupRepo;
        }

        public GasLog UpdateGasLog(GasLog gasLog)
        {
            var catalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == AppConstant.GasLogCatalogName);
            if (catalog != null)
            {
                gasLog.Catalog = catalog;
            }

            SetEntityContent(gasLog);
            var note = _noteManager.Update(gasLog);

            if (note == null)
            {
                SetProcessResult(_noteManager.ProcessResult);
                return null;
            }

            var updatedGasLog = GetEntityFromNote(note);
            return updatedGasLog;
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        public GasLog FindGasLog(int id)
        {
            var note = _noteManager.GetNoteById(id);
            if (note == null)
            {
                SetProcessResult(_noteManager.ProcessResult);
                return null;
            }

            var gasLog = GetEntityFromNote(note);
            return gasLog;
        }

        public GasLog CreateLog(GasLog gasLog)
        {
            var catalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == AppConstant.GasLogCatalogName);
            if (catalog != null)
            {
                gasLog.Catalog = catalog;
            }

            SetEntityContent(gasLog);
            var note = _noteManager.Create(gasLog);

            if (note == null)
            {
                SetProcessResult(_noteManager.ProcessResult);
                return null;
            }

            var newGasLog = GetEntityFromNote(note);
            return newGasLog;
        }

        public GasLog CreateLogForAuthor(int authorId, GasLog gasLog)
        {
            var author = _lookupRepo.GetEntity<User>(authorId);
            if (author == null)
            {
                ProcessResult.Success = false;
                ProcessResult.AddErrorMessage($"Cannot found author with Id {authorId}");
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
                new XElement("GasStation", gasLog.Station),
                new XElement("Discounts", "")
            );

            if (gasLog.Discounts.Any())
            {
                foreach (var disc in gasLog.Discounts)
                {
                    if (disc.Amount == null || disc.Program == null)
                    {
                        ProcessResult.AddErrorMessage("Cannot found valid discount information, amount or discount program is missing");
                        continue;
                    }

                    var discElement = new XElement("Discount",
                        new XElement("Amount", disc.Amount?.Measure2Xml(_noteManager.ContentNamespace)),
                        new XElement("Program", disc.Program?.Id));
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

            // get automobile information
            var carIdStr = logRoot.Element(ns + "Automobile")?.Value;
            int.TryParse(carIdStr, out var carId);
            var car = _carManager.GetAutomobiles().FirstOrDefault(c => c.Id == carId);

            var gas = new GasLog
            {
                Id = note.Id,
                Author = note.Author,
                Catalog = note.Catalog,
                CreateDate = note.CreateDate,
                LastModifiedDate = note.LastModifiedDate,
                Car = car,
                Content = note.Content,
                Station = logRoot.Element(ns + "GasStation")?.Value,
                Description = note.Description,
                Distance = Dimension.FromXml(logRoot.Element(ns + "Distance")?.Element(ns + "Dimension")),
                Gas = Volume.FromXml(logRoot.Element(ns + "Gas")?.Element(ns + "Volume")),
                Price = Money.FromXml(logRoot.Element(ns + "Price")?.Element(ns + "Money"))
            };

            gas.Discounts = _discountManager.GetDiscountInfos(gas).ToList();
            return gas;
        }

        private void SetProcessResult(ProcessingResult innerProcessResult)
        {
            if (innerProcessResult == null)
            {
                return;
            }

            ProcessResult.Rest();
            ProcessResult.Success = innerProcessResult.Success;
            ProcessResult.MessageList.AddRange(innerProcessResult.MessageList);
        }
    }
}