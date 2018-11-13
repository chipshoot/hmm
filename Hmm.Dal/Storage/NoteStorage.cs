using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using Hmm.Utility.Dal.Exceptions;

namespace Hmm.Dal.Storage
{
    public class NoteStorage : StorageBase<HmmNote>
    {
        public NoteStorage(
            IUnitOfWork uow,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, lookupRepo, dateTimeProvider)
        {
        }

        public override HmmNote Add(HmmNote entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // check if need apply default catalog
                // ReSharper disable once PossibleNullReferenceException
                var catalog = PropertyChecking(entity.Catalog);
                entity.Catalog = catalog ?? throw new DataSourceException("Cannot find default note catalog.");

                entity.CreateDate = DateTimeProvider.UtcNow;
                entity.LastModifiedDate = DateTimeProvider.UtcNow;
                var newRec = UnitOfWork.Add(entity);
                return newRec;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddMessage(ex.GetAllMessage(), true);
                return null;
            }
        }

        public override bool Delete(HmmNote entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                UnitOfWork.Delete(entity);
                return true;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddMessage(ex.GetAllMessage(), true);
                return false;
            }
        }

        public override HmmNote Update(HmmNote entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // check if need apply default catalog
                // ReSharper disable once PossibleNullReferenceException
                var catalog = PropertyChecking(entity.Catalog);
                entity.Catalog = catalog ?? throw new DataSourceException("Cannot find default note catalog.");

                entity.LastModifiedDate = DateTimeProvider.UtcNow;
                UnitOfWork.Update(entity);

                var savedRec = LookupRepo.GetEntity<HmmNote>(entity.Id);

                return savedRec;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddMessage(ex.GetAllMessage(), true);
                return null;
            }
        }
    }
}