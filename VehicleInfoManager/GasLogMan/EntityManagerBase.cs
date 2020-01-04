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
    public abstract class EntityManagerBase<T> : IAutoEntityManager<T> where T : VehicleBase
    {
        private readonly IHmmNoteManager<HmmNote> _noteManager;
        private readonly IEntityLookup _lookupRepo;

        protected EntityManagerBase(IHmmNoteManager<HmmNote> noteManager, IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(noteManager == null, nameof(noteManager));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));
            _noteManager = noteManager;
            _lookupRepo = lookupRepo;
        }

        protected XNamespace ContentNamespace => _noteManager.ContentNamespace;

        protected abstract string GetNoteContent(T entity);

        #region method of interface IAutoEntityManager

        public abstract IQueryable<T> GetEntities();

        public abstract T GetEntityById(int id);

        public abstract T Create(T entity, User author);

        public abstract T Update(T entity, User author);

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        #endregion method of interface IAutoEntityManager

        protected abstract T GetEntityFromRawData(HmmNote note);

        protected IQueryable<T> GetEntitiesFromRawData(string subject)
        {
            var notes = _noteManager.GetNotes()
                .Where(n => n.Subject == subject)
                .Select(GetEntityFromRawData).AsQueryable();
            return notes;
        }

        protected int CreateEntityRawData(T entity, string catalogName, User author)
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
                CreateDate = DateTime.UtcNow
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
            curNote.LastModifiedDate = DateTime.Now;
            var newNode = _noteManager.Update(curNote);

            if (!_noteManager.ProcessResult.Success || newNode == null)
            {
                ProcessResult.PropagandaResult(_noteManager.ProcessResult);
            }
        }
    }
}