using Hmm.Utility.Dal.DataEntity;

namespace Hmm.Utility.Dal.Query
{
    /// <summary>
    /// The interface is for quick check entity by its identity
    /// </summary>
    public interface IEntityLookup
    {
        T GetEntity<T>(int id) where T : Entity;
    }
}