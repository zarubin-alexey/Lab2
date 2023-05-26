using Lab2.Models;
using System;

namespace Lab2.Exceptions
{
    /// <summary>
    /// The base class for the all custom error exceptions.
    /// </summary>
    public abstract class HttpException : Exception
    {
        #region Properties

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public ErrorCodes ErrorCode { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate a new instance of the <see cref="HttpException"/>.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="errorMessage">Exception message.</param>
        public HttpException(ErrorCodes errorCode, string errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        #endregion
    }
}
