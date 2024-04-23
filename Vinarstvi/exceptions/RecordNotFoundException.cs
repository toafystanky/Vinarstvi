using System;

namespace Vinarstvi.exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a record is not found.
    /// </summary>
    public class RecordNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        public RecordNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class with the specified message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public RecordNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RecordNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}