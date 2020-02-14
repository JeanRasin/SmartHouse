using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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

        public ExceptionMiddleware(RequestDelegate next, ILogger log)
        {
            this.next = next;
            this.log = log;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch(ModelStateException ex)
            {
                log.LogError("Goal model is not valid.");
                await HandleModelStateExceptionAsync(httpContext, ex);
            }
            catch(NotFoundException ex)
            {
                log.LogError($"Data not found: {ex}");
                await HandleExceptionNotFoundAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                log.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
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
                Message = "Goal model is not valid."
            }.ToString());
        }
    }
}
