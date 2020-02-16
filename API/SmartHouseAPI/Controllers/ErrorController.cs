using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouseAPI.ApiException;
using System;

namespace SmartHouseAPI.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error == null)
            {
                return base.Problem(detail: context.Error?.StackTrace, title: context.Error.Message);
            }

            if (context.Error is NotFoundException)
            {
                ObjectResult result = Problem(detail: context.Error.StackTrace, title: context.Error.Message, statusCode: StatusCodes.Status404NotFound);
                // result.

                return result;
            }
            else if (context.Error is ModelStateException)
            {
                ObjectResult result = Problem(detail: context.Error.StackTrace, title: context.Error.Message, statusCode: StatusCodes.Status400BadRequest);
                return result;
            }
            else
            {
                return base.Problem(detail: context.Error?.StackTrace, title: context.Error?.Message);
            }
        }

        [Route("/error")]
        // public IActionResult Error() => Problem();
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error == null)
            {
                return base.Problem();
            }

            if (context.Error is NotFoundException)
            {
                ObjectResult result = Problem(statusCode: StatusCodes.Status404NotFound);

                return result;
            }
            else if (context.Error is ModelStateException)
            {
                ObjectResult result = Problem(statusCode: StatusCodes.Status400BadRequest);
                return result;
            }
            else
            {
                return base.Problem();
            }
        }
    }
}