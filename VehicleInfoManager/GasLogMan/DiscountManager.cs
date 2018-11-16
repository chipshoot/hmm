using DomainEntity.Misc;
using DomainEntity.Vehicle;
using Hmm.Contract;
using Hmm.Contract.Core;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public class DiscountManager : ManagerBase<GasDiscount>, IDiscountManager
    {
        private readonly IHmmNoteManager<HmmNote> _noteManager;
        private readonly IEntityLookup _lookupRepo;

        public DiscountManager(IHmmNoteManager<HmmNote> noteManager, IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(noteManager == null, nameof(noteManager));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));

            _noteManager = noteManager;
            _lookupRepo = lookupRepo;
        }

        public IEnumerable<GasDiscount> GetDiscounts()
        {
            var discounts = _noteManager.GetNotes().Where(n => n.Subject == AppConstant.GasDiscountRecordSubject);
            return discounts.Cast<GasDiscount>().AsEnumerable();
        }

        public GasDiscount GetDiscountById(int id)
        {
            return GetDiscounts().FirstOrDefault(d => d.Id == id);
        }

        public GasDiscount CreateDiscount(GasDiscount discount)
        {
            Guard.Against<ArgumentNullException>(discount == null, nameof(discount));

            var discountCatalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == AppConstant.GasDiscountRecordSubject);
            if (discountCatalog == null)
            {
                ProcessResult.Rest();
                ProcessResult.Success = false;
                ProcessResult.AddMessage("Cannot find discount catalog from data source");
                return null;
            }

            // ReSharper disable once PossibleNullReferenceException
            discount.Subject = AppConstant.GasDiscountRecordSubject;
            discount.Catalog = discountCatalog;

            SetEntityContent(discount);
            var savedDiscount = _noteManager.Create(discount);

            if (!_noteManager.ProcessResult.Success)
            {
                ProcessResult.Rest();
                ProcessResult.Success = false;
                ProcessResult.MessageList.AddRange(_noteManager.ProcessResult.MessageList);
                return null;
            }

            return savedDiscount as GasDiscount;
        }

        public GasDiscount UpdateDiscount(GasDiscount discount)
        {
            Guard.Against<ArgumentNullException>(discount == null, nameof(discount));

            // ReSharper disable once PossibleNullReferenceException
            SetEntityContent(discount);
            var savedDiscount = _noteManager.Update(discount);

            if (!_noteManager.ProcessResult.Success)
            {
                ProcessResult.Rest();
                ProcessResult.Success = false;
                ProcessResult.MessageList.AddRange(_noteManager.ProcessResult.MessageList);
                return null;
            }

            return savedDiscount as GasDiscount;
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        protected override void SetEntityContent(GasDiscount discount)
        {
            var xml = new XElement(AppConstant.GasDiscountRecordSubject,
                new XElement("Date", discount.CreateDate.ToString("O")),
                new XElement("Program", discount.Program),
                new XElement("Amount", discount.Amount.Measure2Xml(_noteManager.ContentNamespace)),
                new XElement("DiscountType", discount.DiscountType),
                new XElement("IsActive", discount.IsActive),
                new XElement("Comment", discount.Comment)
            );

            discount.Content = string.Empty;
            discount.Content = xml.ToString(SaveOptions.DisableFormatting);
        }

        protected override GasDiscount GetEntityFromNote(HmmNote note)
        {
            throw new System.NotImplementedException();
        }
    }
}