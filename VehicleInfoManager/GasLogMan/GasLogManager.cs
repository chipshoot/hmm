using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.Utility.Currency;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public class GasLogManager : EntityManagerBase<GasLog>
    {
        private readonly IAutoEntityManager<Automobile> _carManager;
        private readonly IAutoEntityManager<GasDiscount> _discountManager;

        public GasLogManager(
            IHmmNoteManager noteManager,
            IAutoEntityManager<Automobile> carManager,
            IAutoEntityManager<GasDiscount> discountManager,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateProvider) : base(noteManager, lookupRepo, dateProvider)
        {
            Guard.Against<ArgumentNullException>(carManager == null, nameof(carManager));
            Guard.Against<ArgumentNullException>(discountManager == null, nameof(discountManager));
            _carManager = carManager;
            _discountManager = discountManager;
        }

        public override IEnumerable<GasLog> GetEntities()
        {
            var logs = GetEntitiesFromRawData(AppConstant.GasLogRecordSubject);
            foreach (var log in logs)
            {
                log.Discounts = FillDiscounts(log.Discounts);
            }
            return logs;
        }

        public override GasLog GetEntityById(int id)
        {
            return GetEntities().FirstOrDefault(l => l.Id == id);
        }

        public override GasLog Create(GasLog gasLog, User author)
        {
            Guard.Against<ArgumentNullException>(gasLog == null, nameof(gasLog));

            var id = CreateEntityRawData(gasLog, AppConstant.GasLogRecordSubject, author);
            if (!ProcessResult.Success)
            {
                return null;
            }

            var savedGasLog = GetEntityById(id);
            return savedGasLog;
        }

        public override GasLog Update(GasLog gasLog, User author)
        {
            Guard.Against<ArgumentNullException>(gasLog == null, nameof(gasLog));

            // ReSharper disable once PossibleNullReferenceException
            var curLog = GetEntityById(gasLog.Id);
            if (curLog == null)
            {
                ProcessResult.AddErrorMessage("Cannot find gas log in data source");
                return null;
            }

            curLog.Car = gasLog.Car;
            curLog.Gas = gasLog.Gas;
            curLog.Price = gasLog.Price;
            curLog.Station = gasLog.Station;
            curLog.Distance = gasLog.Distance;
            curLog.Discounts = gasLog.Discounts;

            UpdateEntityRawData(curLog, AppConstant.GasLogRecordSubject);
            if (!ProcessResult.Success)
            {
                return null;
            }

            var savedDiscount = GetEntityById(curLog.Id);
            return savedDiscount;
        }

        protected override string GetNoteContent(GasLog entity)
        {
            var xml = new XElement("GasLog",
                new XElement("Automobile", entity.Car.Id),
                new XElement("Distance", entity.Distance.Measure2Xml(ContentNamespace)),
                new XElement("Gas", entity.Gas.Measure2Xml(ContentNamespace)),
                new XElement("Price", entity.Price.Measure2Xml(ContentNamespace)),
                new XElement("GasStation", entity.Station),
                new XElement("Discounts", "")
            );

            if (entity.Discounts.Any())
            {
                foreach (var disc in entity.Discounts)
                {
                    if (disc.Amount == null || disc.Program == null)
                    {
                        ProcessResult.AddErrorMessage("Cannot found valid discount information, amount or discount program is missing");
                        continue;
                    }

                    var discElement = new XElement("Discount",
                        new XElement("Amount", disc.Amount?.Measure2Xml(ContentNamespace)),
                        new XElement("Program", disc.Program?.Id));
                    xml.Element("Discounts")?.Add(discElement);
                }
            }

            return xml.ToString(SaveOptions.DisableFormatting);
        }

        protected override GasLog GetEntityFromRawData(HmmNote note)
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
            var car = _carManager.GetEntityById(carId);

            var gas = new GasLog
            {
                Id = note.Id,
                Car = car,
                Station = logRoot.Element(ns + "GasStation")?.Value,
                Distance = Dimension.FromXml(logRoot.Element(ns + "Distance")?.Element(ns + "Dimension")),
                Gas = Volume.FromXml(logRoot.Element(ns + "Gas")?.Element(ns + "Volume")),
                Price = Money.FromXml(logRoot.Element(ns + "Price")?.Element(ns + "Money"))
            };

            var discounts = GetDiscountInfos(logRoot.Element(ns + "Discounts"), ns);
            if (discounts.Any())
            {
                gas.Discounts = discounts;
            }

            return gas;
        }

        private List<GasDiscountInfo> GetDiscountInfos(XElement discountRoot, XNamespace ns)
        {
            var infos = new List<GasDiscountInfo>();
            if (discountRoot == null)
            {
                return infos;
            }

            foreach (var element in discountRoot.Elements())
            {
                var amountNode = element.Element(ns + "Amount")?.Element(ns + "Money");
                if (amountNode == null)
                {
                    ProcessResult.AddErrorMessage("Cannot found money information from discount string");
                    continue;
                }

                var money = Money.FromXml(amountNode);
                var discountIdStr = element.Element(ns + "Program")?.Value;

                if (!int.TryParse(discountIdStr, out var discountId))
                {
                    ProcessResult.AddErrorMessage($"Cannot found valid discount id from string {discountIdStr}");
                    continue;
                }

                // Setup temporary discount object with right id, we cannot get discount entity right now because DB reader
                // is still opening for GasLog
                var discount = _discountManager.GetEntityById(discountId);
                if (discount == null)
                {
                    ProcessResult.AddErrorMessage($"Cannot found discount id : {discountId} from data source");
                    continue;
                }

                infos.Add(new GasDiscountInfo
                {
                    Amount = money,
                    Program = discount
                });
            }

            return infos;
        }

        // Replace place holder gas discount with found gas discount record in data source
        private List<GasDiscountInfo> FillDiscounts(List<GasDiscountInfo> gasDiscounts)
        {
            if (gasDiscounts == null || !gasDiscounts.Any())
            {
                return gasDiscounts;
            }

            foreach (var item in gasDiscounts)
            {
                var discount = _discountManager.GetEntityById(item.Program.Id);
                item.Program = discount;
            }

            return gasDiscounts;
        }
    }
}