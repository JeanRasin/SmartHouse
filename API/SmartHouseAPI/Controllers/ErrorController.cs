using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouseAPI.ApiException;
using System;
using System.Collections.Generic;

namespace SmartHouseAPI.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error-local-development")]
        [HttpGet]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
            }
           // var result0 = Problem();

          // var kk = new ObjectResult("777");
            // return
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error == null)
            {
                //var result = base.Problem(detail: context.Error?.StackTrace, title: context.Error.Message);
                var result = Problem();
                //var result = base.HttpContext;
                return result;
            }

            if (context.Error is KeyNotFoundException)
            {
                ObjectResult result = Problem(detail: context.Error.StackTrace, title: context.Error.Message, statusCode: StatusCodes.Status404NotFound);
                return result;
            }
            else if (context.Error is ModelStateException)
            {
                ObjectResult result = Problem(detail: context.Error.StackTrace, title: context.Error.Message, statusCode: StatusCodes.Status400BadRequest);
                return result;
            }
            else
            {
                ObjectResult result = Problem(detail: context.Error?.StackTrace, title: context.Error?.Message, statusCode: StatusCodes.Status500InternalServerError);
                return result;
            }
        }

        [Route("/error")]
        [HttpGet]
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
                return Problem();
            }
        }

        public override ObjectResult Problem(string detail = null,string instance = null,int? statusCode = null,string title = null,string type = null)
        {
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: statusCode ?? 500,
                title: title,
                type: type,
                detail: detail,
                instance: instance);

            return new ObjectResult(problemDetails);
        }
    }
}