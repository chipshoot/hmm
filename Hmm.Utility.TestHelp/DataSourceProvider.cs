using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Utility.TestHelp
{
    public class DataSourceProvider : IDataSourceProvider
    {
        public DataSourceProvider(Func<IEntityLookup> lookup, Func<IUnitOfWork> uow, Func<IDateTimeProvider> dateProvider)
        {
            Guard.Against<ArgumentNullException>(lookup == null, nameof(lookup));
            Guard.Against<ArgumentNullException>(uow == null, nameof(uow));
            Guard.Against<ArgumentNullException>(dateProvider == null, nameof(dateProvider));

            // ReSharper disable PossibleNullReferenceException
            Lookup = lookup();
            UnitOfWork = uow();
            DateTimeAdapter = dateProvider();
            // ReSharper restore PossibleNullReferenceException
        }

        public IEntityLookup Lookup { get; }

        public IUnitOfWork UnitOfWork { get; }

        public IDateTimeProvider DateTimeAdapter { get; }
    }
}