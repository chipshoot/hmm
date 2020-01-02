using Hmm.Utility.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hmm.Utility.Dal.Repository
{
    /// <summary>
    /// The <see cref="IGenericRepository{T}"/> interface defines a standard contract that repository
    /// components should implement.
    /// </summary>
    /// <typeparam name="T">The entity type we want to managed in the repository</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Grab all item in a ItemRepository
        /// </summary>
        /// <typeparam name="T">The item type we want to find</typeparam>
        /// <returns>The List of items found in parent</returns>
        IQueryable<T> FindEntities();

        /// <summary>
        /// Gets the list of entities that match criteria.
        /// </summary>
        /// <param name="query">The query to search the data source.</param>
        /// <returns>
        /// The list of entity that match the criteria
        /// </returns>
        IEnumerable<T> FindEntities(Expression<Func<T, bool>> query);

        /// <summary>
        /// Adds the entity to data source.
        /// </summary>
        /// <param name="entity">the entity which will be added</param>
        /// <returns>The new added entity with id</returns>
        bool Add(T entity);

        /// <summary>
        /// Updates the specified entity of data source.
        /// </summary>
        /// <param name="entity">The entity which will be updated.</param>
        /// <returns>The new added entity with id</returns>
        bool Update(T entity);

        /// <summary>
        /// Deletes the specified entity from data source.
        /// </summary>
        /// <param name="entity">The entity which will be removed.</param>
        void Delete(T entity);

        /// <summary>
        /// Get processing result, the result may contains error message or
        /// logging message
        /// </summary>
        ProcessingResult ProcessMessage { get; }
    }
}