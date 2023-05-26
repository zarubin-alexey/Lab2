using System;
using System.Net;
using System.Threading.Tasks;
using Lab2.Exceptions;
using Lab2.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Lab2
{
    namespace PropertyMon.API.Middlewares
    {
        /// <summary>
        /// Middleware class for the exceptions handling.
        /// </summary>
        public class ExceptionMiddleware
        {
            #region Constructors

            /// <summary>
            /// Instantiate new instance of the <see cref="ExceptionMiddleware"/> class.
            /// </summary>
            /// <param name="next">Function that can process an HTTP request.</param>
            public ExceptionMiddleware(RequestDelegate next)
            {
                this.next = next;
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Invokes username property into log context.
            /// </summary>
            /// <param name="httpContext">HTTP information about HTTP requests.</param>
            /// <returns>A task that represents the completion of request processing.</returns>
            public async Task Invoke(HttpContext httpContext)
            {
                try
                {
                    await next(httpContext);
                }
                catch (Exception exception)
                {
                    await HandleException(httpContext, exception);
                }
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Hadles an exception from the server.
            /// </summary>
            /// <param name="httpContext">HTTP information about HTTP requests.</param>
            /// <param name="exception">Server exception.</param>
            /// <returns>A task that represents the completion of exception handling.</returns>
            private static async Task HandleException(HttpContext httpContext, Exception exception)
            {
                var httpStatusCode = exception switch
                {
                    BadRequestException => HttpStatusCode.BadRequest,
                    InternalServerErrorException => HttpStatusCode.InternalServerError,
                    NotFoundException => HttpStatusCode.NotFound,
                    _ => HttpStatusCode.InternalServerError,
                };

                await SendException(httpContext, httpStatusCode, exception);
            }

            /// <summary>
            /// Sets up the response status code, content type and return a response.
            /// </summary>
            /// <param name="httpContext">HTTP request context.</param>
            /// <param name="httpStatusCode">HTTP status code.</param>
            /// <param name="exception">Exception.</param>
            /// <returns>Async operation.</returns>
            private static async Task SendException(HttpContext httpContext, HttpStatusCode httpStatusCode, Exception exception)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)httpStatusCode;

                ErrorResponse errorResponse;

                if (exception is HttpException ex)
                {
                    errorResponse = new ErrorResponse(ex.ErrorCode, ex.ErrorMessage);
                }
                else
                {
                    errorResponse = new ErrorResponse(ErrorCodes.UnknownError, "Unknown exceptionss");
                }

                var json = JsonConvert.SerializeObject(errorResponse);

                await httpContext.Response.WriteAsync(json);
            }

            #endregion

            #region Fields

            private readonly RequestDelegate next;

            #endregion
        }
    }

}
