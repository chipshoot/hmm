using Hmm.Utility.Dal.DataEntity;

namespace Hmm.Utility.Dal.DataStore
{
    /// <summary>
    /// The <see cref="IDataStore{TEntity}"/> interface defines a standard contract that repository
    /// components should implement for GRUD, all <see cref="Entity"/> of the repository gets integer as its unique identity
    /// </summary>
    /// <typeparam name="T">The entity type we want to managed in the repository</typeparam>
    public interface IDataStore<T> : IGenericDataStore<T, int> where T : Entity
    {
    }
}