namespace Hmm.Utility.Dal
{
    /// <summary>
    /// The <see cref="IRepository{TEntity}"/> interface defines a standard contract that repository
    /// components should implement, all <see cref="Entity"/> of the repository gets integer as its unique id
    /// </summary>
    /// <typeparam name="T">The entity type we want to managed in the repository</typeparam>
    public interface IRepository<T> : IGenericRepository<T> where T : Entity
    {
        /// <summary>
        /// Finds the entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Entity"/>.</param>
        /// <returns><see cref="Entity"/> with id has been found from data source, otherwise null</returns>
        T FindEntityById(int id);
    }
}