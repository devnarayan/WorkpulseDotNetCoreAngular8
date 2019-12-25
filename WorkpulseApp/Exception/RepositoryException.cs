using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WorkpulseApp.Exception
{
    /// <summary>
	/// RepositoryException is thrown from inside the Repository namespace.  It includes the inner exception that is the root cause of the RepositoryException
	/// </summary>
	[Serializable]
    public class RepositoryException : System.Exception
    {
        private static string _notUsedMessage;

        /// <summary>
        /// This ctor is not used
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public RepositoryException()
        {
            _notUsedMessage = "This ctor is not used";
            throw new NotImplementedException(_notUsedMessage);
        }

        /// <summary>
        /// This ctor is not used
        /// </summary>
        /// <param name="theMessage"></param>
        /// <exception cref="NotImplementedException"></exception>
        public RepositoryException(string theMessage) : base(theMessage)
        {

        }

        /// <summary>
        /// This ctor is not used
        /// </summary>
        /// <param name="theSerializationInfo"></param>
        /// <param name="theStreamingContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected RepositoryException(SerializationInfo theSerializationInfo, StreamingContext theStreamingContext) : base(theSerializationInfo, theStreamingContext)
        {
            throw new NotImplementedException(_notUsedMessage);
        }


        /// <summary>
        /// ctor for RespositoryException
        /// </summary>
        /// <param name="theMessage">Message from place where exception is thrown</param>
        /// <param name="theInnerException">The root cause of the RepositoryException</param>
        public RepositoryException(string theMessage, System.Exception theInnerException) : base(theMessage, theInnerException)
        {

        }
    }
}
