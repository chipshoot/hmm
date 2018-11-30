using System;
using DomainEntity.Misc;
using Hmm.Contract.Core;
using Hmm.Utility.Misc;
using System.Collections.Generic;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Validation;
using Hmm.Core.Manager.Validation;

namespace Hmm.Core.Manager
{
    public class NoteCatalogManager : INoteCatalogManager
    {
        private readonly IDataStore<NoteCatalog> _dataSource;
        private readonly NoteCatalogValidator _validator;

        public NoteCatalogManager(IDataStore<NoteCatalog> dataSource, NoteCatalogValidator validator)
        {
            Guard.Against<ArgumentNullException>(dataSource == null, nameof(dataSource));
            Guard.Against<ArgumentNullException>(validator == null, nameof(validator));

            _dataSource = dataSource;
            _validator = validator;
        }

        public NoteCatalog Create(NoteCatalog catalog)
        {
            if (!_validator.IsValidEntity(catalog, ProcessResult))
            {
                return null;
            }

            try
            {
                var addedCatalog = _dataSource.Add(catalog);
                if (addedCatalog == null)
                {
                    ProcessResult.PropagandaResult(_dataSource.ProcessMessage);
                }
                return addedCatalog;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public NoteCatalog Update(NoteCatalog catalog)
        {
            if (!_validator.IsValidEntity(catalog, ProcessResult))
            {
                return null;
            }

            var updatedCatalog = _dataSource.Update(catalog);
            if (updatedCatalog == null)
            {
                ProcessResult.PropagandaResult(_dataSource.ProcessMessage);
            }

            return updatedCatalog;
        }

        public IEnumerable<NoteCatalog> GetEntities()
        {
            try
            {
                var catalogs = _dataSource.GetEntities();
                return catalogs;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();
    }
}