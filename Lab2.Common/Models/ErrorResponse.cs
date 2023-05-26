using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class ErrorResponse
    {
        #region Properties

        /// <summary>
        /// Gets the error code.
        /// </summary>
        [JsonProperty("errorCode")]
        public ErrorCodes ErrorCode { get; set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        public ErrorResponse()
        { }

        /// <summary>
        /// Instantiate new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="errorMessage">Error message.</param>
        public ErrorResponse(ErrorCodes errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        #endregion
    }
}
