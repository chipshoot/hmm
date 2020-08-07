using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public class AutomobileManager : EntityManagerBase<Automobile>
    {
        public AutomobileManager(IHmmNoteManager noteManager, IEntityLookup lookupRepo, IDateTimeProvider dateProvider) : base(noteManager, lookupRepo, dateProvider)
        {
        }

        public override IEnumerable<Automobile> GetEntities()
        {
            return GetEntitiesFromRawData(AppConstant.AutoMobileRecordSubject);
        }

        public override Automobile GetEntityById(int id)
        {
            var car = GetEntities().FirstOrDefault(c => c.Id == id);
            return car;
        }

        public override Automobile Create(Automobile car, User author)
        {
            Guard.Against<ArgumentNullException>(car == null, nameof(car));

            var id = CreateEntityRawData(car, AppConstant.AutoMobileRecordSubject, author);
            if (!ProcessResult.Success)
            {
                return null;
            }

            var savedCar = GetEntityById(id);
            return savedCar;
        }

        public override Automobile Update(Automobile car, User author)
        {
            Guard.Against<ArgumentNullException>(car == null, nameof(car));

            // ReSharper disable once PossibleNullReferenceException
            var curCar = GetEntityById(car.Id);
            if (curCar == null)
            {
                ProcessResult.AddErrorMessage("Cannot find automobile in data source");
                return null;
            }

            curCar.Brand = car.Brand;
            curCar.Maker = car.Maker;
            curCar.MeterReading = car.MeterReading;
            curCar.Pin = car.Pin;
            curCar.Year = car.Year;

            UpdateEntityRawData(curCar, AppConstant.AutoMobileRecordSubject);
            if (!ProcessResult.Success)
            {
                return null;
            }

            var savedCar = GetEntityById(curCar.Id);
            return savedCar;
        }

        protected override string GetNoteContent(Automobile automobile)
        {
            var xml = new XElement(AppConstant.AutoMobileRecordSubject,
                new XElement("Maker", automobile.Maker),
                new XElement("Brand", automobile.Brand),
                new XElement("Year", automobile.Year),
                new XElement("Color", automobile.Color),
                new XElement("Pin", automobile.Pin),
                new XElement("MeterReading", automobile.MeterReading)
            );

            return xml.ToString();
        }

        protected override Automobile GetEntityFromRawData(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var noteStr = note.Content;
            var noteXml = XDocument.Parse(noteStr);
            var ns = noteXml.Root?.GetDefaultNamespace();
            var automobileRoot = noteXml.Root?.Element(ns + "Content")?.Element(ns + AppConstant.AutoMobileRecordSubject);
            if (automobileRoot == null)
            {
                return null;
            }

            int.TryParse(automobileRoot.Element(ns + "MeterReading")?.Value, out var meterReading);
            var automobile = new Automobile
            {
                Id = note.Id,
                MeterReading = meterReading,
                Brand = automobileRoot.Element(ns + "Brand")?.Value,
                Maker = automobileRoot.Element(ns + "Maker")?.Value,
                Year = automobileRoot.Element(ns + "Year")?.Value,
                Pin = automobileRoot.Element(ns + "Pin")?.Value,
            };

            return automobile;
        }
    }
}