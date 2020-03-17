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
            //catch (ModelStateException ex)
            //{
            //    _logger.LogError(new EventId(2), ex, "ModelStateException");
            //    throw ex;
            //}
            //catch (KeyNotFoundException ex)
            //{
            //    _logger.LogError(new EventId(1), ex, "NotFoundException");
            //    throw ex;
            //}
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, "Exception");
                throw ex;
            }
        }
    }
}
