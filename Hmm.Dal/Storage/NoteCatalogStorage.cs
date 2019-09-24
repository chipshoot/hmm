using DomainEntity.Misc;
using Hmm.Dal.Data;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Storage
{
    public class NoteCatalogStorage : StorageBase<NoteCatalog>
    {
        public NoteCatalogStorage(
            IUnitOfWork uow,
            IEntityLookup lookupRepo,
            IDateTimeProvider dateTimeProvider) : base(uow, lookupRepo, dateTimeProvider)
        {
        }

        public override NoteCatalog Add(NoteCatalog entity)
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

        public override NoteCatalog Update(NoteCatalog entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            // ReSharper disable once PossibleNullReferenceException
            if (entity.Id <= 0)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddErrorMessage($"Can not update NoteCatalog with id {entity.Id}", true);
                return null;
            }

            try
            {
                // check if need apply default render
                var render = PropertyChecking(entity.Render);
                const string message = "Cannot find default note render.";
                ProcessMessage.Success = false;
                ProcessMessage.AddErrorMessage(message, true);
                entity.Render = render ?? throw new DataSourceException(message);

                UnitOfWork.Update(entity);
                return LookupRepo.GetEntity<NoteCatalog>(entity.Id);
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public override bool Delete(NoteCatalog entity)
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