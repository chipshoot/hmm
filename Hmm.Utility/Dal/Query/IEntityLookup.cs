using Hmm.Utility.Dal.DataEntity;
using System.Collections.Generic;

namespace Hmm.Utility.Dal.Query
{
    /// <summary>
    /// The interface is for quick check entity by its identity
    /// </summary>
    public interface IEntityLookup
    {
        T GetEntity<T>(int id) where T : Entity;

        IEnumerable<T> GetEntities<T>() where T : Entity;
    }
}