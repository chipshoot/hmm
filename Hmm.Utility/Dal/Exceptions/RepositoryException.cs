using System;

namespace Hmm.Utility.Dal.Exceptions
{
    /// <summary>
    /// Class RepositoryException is thrown whenever there is any error occurred
    /// during processing data throw repository
    /// </summary>
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public RepositoryException(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RepositoryException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }
    }
}