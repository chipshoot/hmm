using System;

namespace Hmm.Utility.Dal
{
    /// <summary>
    /// The Unit of work base contract
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <typeparam name="T">the type of item</typeparam>
        /// <param name="entity">The item we want to add.</param>
        /// <returns>The new entity just added to the system</returns>
        T Add<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <typeparam name="T">the type of the item to be deleted</typeparam>
        /// <param name="entity">The entity which will be deleted.</param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <typeparam name="T">the type of the item to update</typeparam>
        /// <param name="entity">The entity we want to update.</param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <returns>The instance of <see cref="IGenericTransaction"/></returns>
        IGenericTransaction BeginTransaction();

        /// <summary>
        /// Refreshes the data to database.
        /// </summary>
        void Flush();
    }
}