using Hmm.Dal.Data;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Utility.TestHelp
{
    public class DataRepositoryProvider : IDataRepositoryProvider
    {
        public DataRepositoryProvider(Func<IEntityLookup> lookup, Func<IHmmDataContext> dataContext, Func<IDateTimeProvider> dateProvider)
        {
            Guard.Against<ArgumentNullException>(lookup == null, nameof(lookup));
            Guard.Against<ArgumentNullException>(dataContext == null, nameof(dataContext));
            Guard.Against<ArgumentNullException>(dateProvider == null, nameof(dateProvider));

            // ReSharper disable PossibleNullReferenceException
            Lookup = lookup();
            DataContext = dataContext();
            DateTimeAdapter = dateProvider();
            // ReSharper restore PossibleNullReferenceException
        }

        public IEntityLookup Lookup { get; }

        public IHmmDataContext DataContext { get; }

        public IDateTimeProvider DateTimeAdapter { get; }
    }
}