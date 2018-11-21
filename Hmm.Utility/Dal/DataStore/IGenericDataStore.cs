using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Misc;
using System.Collections.Generic;

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
        /// Gets the entities from data source.
        /// </summary>
        /// <returns>The list of entities with type {T}</returns>
        IEnumerable<T> GetEntities();

        /// <summary>
        /// Gets the process message.
        /// </summary>
        /// <value>
        /// The process message.
        /// </value>
        ProcessingResult ProcessMessage { get; }
    }
}