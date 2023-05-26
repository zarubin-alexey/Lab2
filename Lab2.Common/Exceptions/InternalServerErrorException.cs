using Lab2.Models;

namespace Lab2.Exceptions
{
    /// <summary>
    /// Thrown when internal server error was occured.
    /// </summary>
    public class InternalServerErrorException : HttpException
    {
        #region Constructor

        /// <summary>
        /// Instantiate a new instance of the <see cref="InternalServerErrorException"/>.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="errorMessage">Exception message.</param>
        public InternalServerErrorException(ErrorCodes errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        { }

        #endregion
    }
}
