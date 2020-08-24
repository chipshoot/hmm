using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VehicleInfoManager.GasLogMan
{
    public abstract class EntityManagerBase<T> : IAutoEntityManager<T> where T : VehicleBase
    {
        private readonly IHmmNoteManager _noteManager;
        private readonly IEntityLookup _lookupRepo;
        private readonly IDateTimeProvider _dateProvider;

        protected EntityManagerBase(IHmmNoteManager noteManager, IEntityLookup lookupRepo, IDateTimeProvider dateProvider)
        {
            Guard.Against<ArgumentNullException>(noteManager == null, nameof(noteManager));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));
            Guard.Against<ArgumentNullException>(dateProvider == null, nameof(dateProvider));
            _noteManager = noteManager;
            _lookupRepo = lookupRepo;
            _dateProvider = dateProvider;
        }

        protected XNamespace ContentNamespace => _noteManager.ContentNamespace;

        protected abstract string GetNoteContent(T entity);

        #region method of interface IAutoEntityManager

        public abstract IEnumerable<T> GetEntities(User author);

        public abstract T GetEntityById(int id);

        public abstract T Create(T entity, User author);

        public abstract T Update(T entity, User author);

        public bool IsEntityOwner(int id, Guid authorId)
        {
            var hasEntity = GetEntities(null).Any(e => e.Id == id && e.AuthorId == authorId);
            return hasEntity;
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        #endregion method of interface IAutoEntityManager

        protected abstract T GetEntityFromRawData(HmmNote note);

        protected IEnumerable<T> GetEntitiesFromRawData(string subject, User author)
        {
            IEnumerable<T> notes;
            if (author == null)
            {
                notes = _noteManager.GetNotes()
                    .Where(n => n.Subject == subject)
                    .Select(GetEntityFromRawData);
            }
            else
            {
                notes = _noteManager.GetNotes()
                    .Where(n => n.Subject == subject && n.Author.Id == author.Id)
                    .Select(GetEntityFromRawData);
            }

            return notes;
        }

        protected int CreateEntityRawData(T entity, string catalogName, User author, string comment = null)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(catalogName), nameof(entity));

            var catalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == catalogName);
            if (catalog == null)
            {
                ProcessResult.AddErrorMessage("Cannot find discount catalog from data source");
                return -1;
            }

            var note = new HmmNote
            {
                Subject = catalogName,
                Catalog = catalog,
                Author = author,
                Content = GetNoteContent(entity),
                CreateDate = _dateProvider.UtcNow,
                Description = comment
            };
            var newNode = _noteManager.Create(note);

            if (!_noteManager.ProcessResult.Success || newNode == null)
            {
                ProcessResult.AddErrorMessage(_noteManager.ProcessResult.GetWholeMessage());
                return -1;
            }

            return newNode.Id;
        }

        protected void UpdateEntityRawData(T entity, string catalogName)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(catalogName), nameof(entity));

            var catalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == catalogName);
            if (catalog == null)
            {
                ProcessResult.AddErrorMessage("Cannot find discount catalog from data source");
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            var curNote = _noteManager.GetNoteById(entity.Id);
            if (curNote == null)
            {
                ProcessResult.AddErrorMessage("Cannot find note record in data source");
                return;
            }

            curNote.Content = GetNoteContent(entity);
            curNote.LastModifiedDate = _dateProvider.UtcNow;
            var newNode = _noteManager.Update(curNote);

            if (!_noteManager.ProcessResult.Success || newNode == null)
            {
                ProcessResult.PropagandaResult(_noteManager.ProcessResult);
            }
        }
    }
}