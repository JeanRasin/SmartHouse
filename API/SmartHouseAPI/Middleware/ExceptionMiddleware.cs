using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartHouseAPI.ApiException;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouseAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        private readonly RequestDelegate _next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ModelStateException ex)
            {
                _logger.LogError(new EventId(2), ex, "ModelStateException");
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(new EventId(1), ex, "NotFoundException");
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, "Exception");
                throw ex;
            }
        }

        /*
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error."
            }.ToString());
        }

        private Task HandleExceptionNotFoundAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var hh = context.Features.Get<IExceptionHandlerFeature>();

            //return context0.Problem(detail: hh.Error?.StackTrace, title: hh.Error.Message);


            var kk = context?.RequestServices?.GetRequiredService<ProblemDetailsFactory>();

            throw exception;

            //var problemDetails = kk.CreateProblemDetails(
            //    context,
            //    statusCode: context.Response.StatusCode,
            //    title: hh.Error.Message,
            //   // type: type,
            //    detail: hh.Error?.StackTrace
            //    //instance: instance
            //    );

          //  problemDetails.

          //  var ff = new ObjectResult(problemDetails);

            //return new ObjectResult(problemDetails)
            //{
            //    StatusCode = problemDetails.Status
            //};

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Data not found."
            }.ToString());
        }

        private Task HandleModelStateExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Model is not valid."
            }.ToString());
        }
        */
    }
}
