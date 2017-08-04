using System;

namespace Hmm.Utility.Dal.Exceptions
{
    /// <summary>
    /// Class UnitOfWorkException
    /// </summary>
    public class UnitOfWorkException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public UnitOfWorkException(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnitOfWorkException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }
    }
}