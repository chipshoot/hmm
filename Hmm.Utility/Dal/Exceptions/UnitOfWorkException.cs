using System;

namespace Hmm.Utility.Dal.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Class UnitOfWorkException
    /// </summary>
    public class UnitOfWorkException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hmm.Utility.Dal.Exceptions.UnitOfWorkException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public UnitOfWorkException(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hmm.Utility.Dal.Exceptions.UnitOfWorkException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnitOfWorkException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }
    }
}