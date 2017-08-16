using System;

namespace Hmm.Dal.Data
{
    public class DataSourceException : Exception
    {
        public DataSourceException(string message) : base(message)
        {
        }

        public DataSourceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}