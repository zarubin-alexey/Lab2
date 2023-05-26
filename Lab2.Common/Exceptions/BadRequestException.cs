using Lab2.Models;

namespace Lab2.Exceptions
{
    /// <summary>
    /// Thrown when bad request was occured.
    /// </summary>
    public class BadRequestException : HttpException
    {
        #region Constructor

        /// <summary>
        /// Instantiate a new instance of the <see cref="BadRequestException"/>.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="errorMessage">Exception message.</param>
        public BadRequestException(ErrorCodes errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        { }

        #endregion
    }
}
