using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Storage
{
    public class NoteRenderStorage : StorageBase<NoteRender>
    {
        public NoteRenderStorage(
            IUnitOfWork uow,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, lookupRepo, dateTimeProvider)
        {
        }

        public override NoteRender Add(NoteRender entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                var newCat = UnitOfWork.Add(entity);
                return newCat;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public override NoteRender Update(NoteRender entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            // ReSharper disable once PossibleNullReferenceException
            if (entity.Id <= 0)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddErrorMessage($"Can not update NoteRender with id {entity.Id}", true);
                return null;
            }

            try
            {
                // make sure the record exists in data source
                UnitOfWork.Update(entity);
                return LookupRepo.GetEntity<NoteRender>(entity.Id);
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public override bool Delete(NoteRender entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                UnitOfWork.Delete(entity);
                return true;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return false;
            }
        }
    }
}