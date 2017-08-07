using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Validation;

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
        /// Get and Set the entity validator which is used for validating entity when insert, update or delete
        /// entity
        /// </summary>
        IValidator<T> Validator { get; set; }

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
    }
}