using Hmm.Dal.Data;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;

namespace Hmm.Utility.TestHelp
{
    /// <summary>
    /// Used for creating testing/real data source environment for test cases
    /// </summary>
    public interface IDataRepositoryProvider
    {
        IEntityLookup Lookup { get; }

        IHmmDataContext DataContext { get; }

        IDateTimeProvider DateTimeAdapter { get; }
    }
}