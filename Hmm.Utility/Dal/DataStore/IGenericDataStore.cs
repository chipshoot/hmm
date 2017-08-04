using Hmm.Utility.Dal.DataEntity;

namespace Hmm.Utility.Dal.DataStore
{
    /// <summary>
    /// The <see cref="IGenericDataStore{T, TIdentity}"/> interface defines a standard contract that repository
    /// components should implement for CRUD.
    /// </summary>
    /// <typeparam name="T">The entity type we want to managed in the repository</typeparam>
    /// <typeparam name="TIdentity">the type of entity identity</typeparam>
    public interface IGenericDataStore<T, in TIdentity> where T : AbstractEntity<TIdentity>
    {
        /// <summary>
        /// Grab item from data store based on its identity
        /// </summary>
        /// <typeparam name="T">The item type we want to find</typeparam>
        /// <returns>The items found in data store with the identity</returns>
        T Get(TIdentity id);

        /// <summary>
        /// Adds the entity to data source.
        /// </summary>
        /// <param name="entity">the entity which will be added</param>
        /// <returns>The new added entity with id</returns>
        T Add(T entity);

        /// <summary>
        /// Updates the specified entity of data source.
        /// </summary>
        /// <param name="entity">The entity which will be updated.</param>
        /// <returns>The new added entity with id</returns>
        T Update(T entity);

        /// <summary>
        /// Deletes the specified entity from data source.
        /// </summary>
        /// <param name="entity">The entity which will be removed.</param>
        /// <returns>True if delete successfully, otherwise false</returns>
        bool Delete(T entity);

        /// <summary>
        /// Clear entity from data cache and let system retrieve data again from data source next time from <see cref="Get"/>.
        /// Client need this after data source get changed
        /// </summary>
        /// <param name="entity">The entity need to be refreshed</param>
        void Refresh(ref T entity);

        /// <summary>
        /// Flushes the cached data to database.
        /// </summary>
        void Flush();
    }
}