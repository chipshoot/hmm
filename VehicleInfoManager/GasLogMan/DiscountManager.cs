using DomainEntity.Enumerations;
using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.Utility.Currency;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public class DiscountManager : EntityManagerBase<GasDiscount>, IDiscountManager
    {
        public DiscountManager(IHmmNoteManager<HmmNote> noteManager, IEntityLookup lookupRepo) : base(noteManager, lookupRepo)
        {
        }

        public IQueryable<GasDiscount> GetDiscounts()
        {
            var discounts = GetEntities(AppConstant.GasDiscountRecordSubject);
            return discounts;
        }

        public GasDiscount GetDiscountById(int id)
        {
            return GetDiscounts().FirstOrDefault(d => d.Id == id);
        }

        public GasDiscount Create(GasDiscount discount, User author)
        {
            Guard.Against<ArgumentNullException>(discount == null, nameof(discount));

            try
            {
                var id = CreateEntity(discount, AppConstant.GasDiscountRecordSubject, author);
                var savedDiscount = GetDiscountById(id);
                return savedDiscount;
            }
            catch (EntityManagerException ex)
            {
                ProcessResult.AddErrorMessage(ex.Message);
                return null;
            }
        }

        public GasDiscount Update(GasDiscount discount)
        {
            Guard.Against<ArgumentNullException>(discount == null, nameof(discount));

            // ReSharper disable once PossibleNullReferenceException
            var curDiscount = GetDiscountById(discount.Id);
            if (curDiscount == null)
            {
                ProcessResult.AddErrorMessage("Cannot find discount in data source");
                return null;
            }

            curDiscount.Amount = discount.Amount;
            curDiscount.Comment = discount.Comment;
            curDiscount.DiscountType = discount.DiscountType;
            curDiscount.IsActive = discount.IsActive;
            curDiscount.Program = discount.Program;

            try
            {
                UpdateEntity(discount,AppConstant.GasDiscountRecordSubject);
                var savedDiscount = GetDiscountById(discount.Id);
                return savedDiscount;
            }
            catch (EntityManagerException ex)
            {
                ProcessResult.AddErrorMessage(ex.Message);
                return null;
            }
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        protected override GasDiscount GetEntity(HmmNote note)
        {
            if (note == null)
            {
                return null;
            }

            var noteStr = note.Content;
            if (string.IsNullOrEmpty(noteStr))
            {
            }
            var noteXml = XDocument.Parse(noteStr);
            var ns = noteXml.Root?.GetDefaultNamespace();
            var discountRoot = noteXml.Root?.Element(ns + "Content")?.Element(ns + AppConstant.GasDiscountRecordSubject);
            if (discountRoot == null)
            {
                return null;
            }

            bool.TryParse(discountRoot.Element(ns + "IsActive")?.Value, out var isActive);
            Enum.TryParse<GasDiscountType>(discountRoot.Element(ns + "DiscountType")?.Value, out var discType);
            var discount = new GasDiscount
            {
                Id = note.Id,
                Program = discountRoot.Element(ns + "Program")?.Value,
                Amount = Money.FromXml(discountRoot.Element(ns + "Amount")?.Element(ns + "Money")),
                DiscountType = discType,
                IsActive = isActive,
                Comment = discountRoot.Element(ns + "Comment")?.Value,
            };

            return discount;
        }

        protected override string GetNoteContent(GasDiscount entity)
        {
            var xml = new XElement(AppConstant.GasDiscountRecordSubject,
                new XElement("Program", entity.Program),
                new XElement("Amount", entity.Amount.Measure2Xml(ContentNamespace)),
                new XElement("DiscountType", entity.DiscountType),
                new XElement("IsActive", entity.IsActive),
                new XElement("Comment", entity.Comment)
            );

            return xml.ToString(SaveOptions.DisableFormatting);
        }

        private bool IsNoteContentValid(string noteContent, out XDocument noteXml)
        {
            if (string.IsNullOrEmpty(noteContent))
            {
                ProcessResult.Success = false;
                ProcessResult.AddErrorMessage("Null or empty note content found", true);
                noteXml = null;
                return false;
            }

            try
            {
                noteXml = XDocument.Parse(noteContent);
                return true;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                noteXml = null;
                return false;
            }
        }
    }
}