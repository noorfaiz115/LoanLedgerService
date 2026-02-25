
using CryptoHub.Application.Common;
using System.Net;

namespace CryptoHub.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this._logger = logger;
            this._next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);


            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid().ToString();
                //Log the Exception in File That We Create to maintain Log
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                //Return Custome Exception

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                //var error = new
                //{
                //    Id = errorId,
                //    ErrorMessage = "Something Went Wrong! We are looking into resolving it. ",
                //};
                var response = ApiResponse<string>.FailureResponse(
                  "Something went wrong",
                  "INTERNAL_SERVER_ERROR",
                  ex.Message
                 );
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
