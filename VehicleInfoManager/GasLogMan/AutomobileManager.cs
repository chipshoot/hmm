using DomainEntity.Misc;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public class AutomobileManager : ManagerBase<Automobile>, IAutomobileManager
    {
        private const string AutoMobileRecordSubject = "Automobile";
        private readonly IHmmNoteManager<HmmNote> _noteManager;

        public AutomobileManager(IHmmNoteManager<HmmNote> noteManager)
        {
            Guard.Against<ArgumentNullException>(noteManager == null, nameof(noteManager));

            _noteManager = noteManager;
        }

        public IEnumerable<Automobile> GetAutomobiles()
        {
            var cars = _noteManager.GetNotes()
                .Where(n => n.Subject == AutoMobileRecordSubject)
                .Select(GetEntityFromNote).ToList();
            return cars;
        }

        public Automobile CreateAutomobile(Automobile car)
        {
            Guard.Against<ArgumentNullException>(car == null, nameof(car));

            // ReSharper disable once PossibleNullReferenceException
            car.Subject = AutoMobileRecordSubject;
            SetEntityContent(car);
            var savedCar = _noteManager.Create(car);

            if (!_noteManager.ProcessResult.Success)
            {
                ProcessResult.Rest();
                ProcessResult.Success = false;
                ProcessResult.MessageList.AddRange(_noteManager.ProcessResult.MessageList);
                return null;
            }

            return savedCar as Automobile;
        }

        public Automobile UpdateAutomobile(Automobile car)
        {
            Guard.Against<ArgumentNullException>(car == null, nameof(car));

            // ReSharper disable once PossibleNullReferenceException
            SetEntityContent(car);
            var savedCar = _noteManager.Update(car);

            if (!_noteManager.ProcessResult.Success)
            {
                ProcessResult.Rest();
                ProcessResult.Success = false;
                ProcessResult.MessageList.AddRange(_noteManager.ProcessResult.MessageList);
                return null;
            }

            return savedCar as Automobile;
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        protected override void SetEntityContent(Automobile automobile)
        {
            var xml = new XElement("Automobile",
                new XElement("Date", automobile.CreateDate.ToString("O")),
                new XElement("MeterReading", automobile.MeterReading),
                new XElement("Brand", automobile.Brand),
                new XElement("Maker", automobile.Maker),
                new XElement("Year", automobile.Year),
                new XElement("Pin", automobile.Pin)
            );

            automobile.Content = string.Empty;
            automobile.Content = xml.ToString(SaveOptions.DisableFormatting);
        }

        protected override Automobile GetEntityFromNote(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var noteStr = note.Content;
            var noteXml = XDocument.Parse(noteStr);
            var ns = noteXml.Root?.GetDefaultNamespace();
            var automobileRoot = noteXml.Root?.Element(ns + "Content")?.Element(ns + "Automobile");
            if (automobileRoot == null)
            {
                return null;
            }

            int.TryParse(automobileRoot.Element(ns + "MeterReading")?.Value, out var meterReading);
            var automobile = new Automobile
            {
                Id = note.Id,
                Author = note.Author,
                Catalog = note.Catalog,
                CreateDate = note.CreateDate,
                LastModifiedDate = note.LastModifiedDate,
                Content = note.Content,
                MeterReading = meterReading,
                Brand = automobileRoot.Element(ns + "Brand")?.Value,
                Maker = automobileRoot.Element(ns + "Maker")?.Value,
                Year = automobileRoot.Element(ns + "Year")?.Value,
                Pin = automobileRoot.Element(ns + "Pin")?.Value,
                Description = note.Description,
            };

            return automobile;
        }
    }
}