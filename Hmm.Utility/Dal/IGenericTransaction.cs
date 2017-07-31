using System;

namespace Hmm.Utility.Dal
{
    public interface IGenericTransaction : IDisposable
    {
        /// <summary>
        /// Commits changes in transaction to database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks all changes in current transaction.
        /// </summary>
        void Rollback();
    }
}