using Lab2.Models;

namespace Lab2.Exceptions
{
    /// <summary>
    /// Thrown when something is not found.
    /// </summary>
    public class NotFoundException : HttpException
    {
        #region Constructor

        /// <summary>
        /// Instantiate a new instance of the <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="errorMessage">Exception message.</param>
        public NotFoundException(ErrorCodes errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        { }

        #endregion
    }
}
