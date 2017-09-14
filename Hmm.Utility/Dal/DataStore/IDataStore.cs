using Hmm.Utility.Dal.DataEntity;

namespace Hmm.Utility.Dal.DataStore
{
    /// <inheritdoc />
    /// <summary>
    /// The <see cref="T:Hmm.Utility.Dal.DataStore.IDataStore`1" /> interface defines a standard contract that repository
    /// components should implement for GRUD, all <see cref="T:Hmm.Utility.Dal.DataEntity.Entity" /> of the repository gets integer as its unique identity
    /// </summary>
    /// <typeparam name="T">The entity type we want to managed in the repository</typeparam>
    public interface IDataStore<T> : IGenericDataStore<T, int> where T : Entity
    {
    }
}