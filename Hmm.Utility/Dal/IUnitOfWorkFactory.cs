namespace Hmm.Utility.Dal
{
    /// <summary>
    /// This is the interface between domain entity to database.
    /// Client can query the entity through this interface and
    /// get UnitOfWork for change the data in database
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        #region methods

        /// <summary>
        /// Gets the current <see cref="IUnitOfWork"/> which against HPC database.
        /// </summary>
        /// <value>
        /// The current HPC connected <see cref="IUnitOfWork"/>.
        /// </value>
        IUnitOfWork CurrentUnitOfWork { get; }

        /// <summary>
        /// Binds the unit of work in certain context for MVC and WCF web services.
        /// </summary>
        void BindUnitOfWork();

        /// <summary>
        /// Closes the unit of work.
        /// </summary>
        void CloseUnitOfWork();

        #endregion methods
    }
}