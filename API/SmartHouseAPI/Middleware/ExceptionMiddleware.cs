using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHouseAPI.ApiException;
using SmartHouseAPI.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SmartHouseAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger log;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            log = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch(ModelStateException ex)
            {
                log.LogError(new EventId(2), ex, "ModelStateException");
                throw ex;
               // await HandleModelStateExceptionAsync(httpContext, ex);
            }
            catch(NotFoundException ex)
            {
                log.LogError(new EventId(1), ex, "NotFoundException");
                throw new Exception("444");
                //await HandleExceptionNotFoundAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                log.LogError(new EventId(0), ex, "Exception");
                throw ex;
                // await HandleExceptionAsync(httpContext, ex);
            }
        }

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
            context.Response.ContentType = "application/json";
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

            //ff.

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
    }
}
