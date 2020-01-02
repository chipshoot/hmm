using DomainEntity.Misc;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;
using System.Linq;
using System.Xml.Linq;
using DomainEntity.User;

namespace VehicleInfoManager.GasLogMan
{
    public abstract class EntityManagerBase<T> where T : VehicleBase
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

        protected abstract T GetEntity(HmmNote note);

        protected IQueryable<T> GetEntities(string subject)
        {
            var notes = _noteManager.GetNotes()
                .Where(n => n.Subject == subject)
                .Select(GetEntity).AsQueryable();
            return notes;
        }

        protected int CreateEntity(T entity, string catalogName, User author)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(catalogName), nameof(entity));

            var catalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == catalogName);
            if (catalog == null)
            {
                throw new EntityManagerException("Cannot find discount catalog from data source");
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
                throw new EntityManagerException(_noteManager.ProcessResult.GetWholeMessage());
            }

            return newNode.Id;
        }

        protected void UpdateEntity(T entity, string catalogName)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(catalogName), nameof(entity));

            var catalog = _lookupRepo.GetEntities<NoteCatalog>().FirstOrDefault(c => c.Name == catalogName);
            if (catalog == null)
            {
                throw new EntityManagerException("Cannot find discount catalog from data source");
            }

            // ReSharper disable once PossibleNullReferenceException
            var curNote = _noteManager.GetNoteById(entity.Id);
            if (curNote == null)
            {
                throw new EntityManagerException("Cannot find note record in data source");
            }

            curNote.Content = GetNoteContent(entity);
            curNote.LastModifiedDate=DateTime.Now;
            var newNode = _noteManager.Update(curNote);

            if (!_noteManager.ProcessResult.Success || newNode == null)
            {
                throw new EntityManagerException(_noteManager.ProcessResult.GetWholeMessage());
            }
        }
    }
}