using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public class AutomobileManager : EntityManagerBase<Automobile>, IAutomobileManager
    {
        public AutomobileManager(IHmmNoteManager<HmmNote> noteManager, IEntityLookup lookupRepo) : base(noteManager, lookupRepo)
        {
        }

        public IQueryable<Automobile> GetAutomobiles()
        {
            return GetEntities(AppConstant.AutoMobileRecordSubject);
        }

        public Automobile GetAutomobileById(int id)
        {
            var car = GetAutomobiles().FirstOrDefault(c => c.Id == id);
            return car;
        }

        public Automobile Create(Automobile car, User author)
        {
            Guard.Against<ArgumentNullException>(car == null, nameof(car));

            try
            {
                var id = CreateEntity(car, AppConstant.AutoMobileRecordSubject, author);
                var savedCar = GetAutomobileById(id);
                return savedCar;
            }
            catch (EntityManagerException ex)
            {
                ProcessResult.AddErrorMessage(ex.Message);
                return null;
            }
        }

        public Automobile Update(Automobile car, User author)
        {
            Guard.Against<ArgumentNullException>(car == null, nameof(car));

            // ReSharper disable once PossibleNullReferenceException
            var curCar = GetAutomobileById(car.Id);
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

            try
            {
                UpdateEntity(curCar, AppConstant.AutoMobileRecordSubject);
                var savedCar = GetAutomobileById(curCar.Id);
                return savedCar;
            }
            catch (EntityManagerException ex)
            {
                ProcessResult.AddErrorMessage(ex.Message);
                return null;
            }
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        protected override string GetNoteContent(Automobile automobile)
        {
            var xml = new XElement(AppConstant.AutoMobileRecordSubject,
                new XElement("MeterReading", automobile.MeterReading),
                new XElement("Brand", automobile.Brand),
                new XElement("Maker", automobile.Maker),
                new XElement("Year", automobile.Year),
                new XElement("Pin", automobile.Pin)
            );

            return xml.ToString();
        }

        protected override Automobile GetEntity(HmmNote note)
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