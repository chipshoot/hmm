using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;

namespace Hmm.Utility.TestHelp
{
    /// <summary>
    /// Used for creating testing/real data source environment for test cases
    /// </summary>
    public interface IDataSourceProvider
    {
        IEntityLookup Lookup { get; }

        IUnitOfWork UnitOfWork { get; }

        IDateTimeProvider DateTimeAdapter { get; }
    }
}