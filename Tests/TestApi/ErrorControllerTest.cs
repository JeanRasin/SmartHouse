using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartHouseAPI.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ApiTest
{
    public class ErrorControllerTest
    {
        //protected T GetController<T>() where T : Controller, new()
        //{
        //    var routes = new RouteCollection();
        //   // MvcApplication.RegisterRoutes(routes);

        //    var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
        //    request.SetupGet(x => x.ApplicationPath).Returns("/");
        //    request.SetupGet(x => x.Url).Returns(new Uri("http://localhost", UriKind.Absolute));
        //    request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

        //    var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
        //    response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string p) => p);

        //    var context = new Mock<HttpContextBase>(MockBehavior.Strict);
        //    context.SetupGet(x => x.Request).Returns(request.Object);
        //    context.SetupGet(x => x.Response).Returns(response.Object);

        //    var controller = new T();
        //    controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        //    controller.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);

        //    return controller;
        //}

        [Fact]
        public void ErrorLocalDevelopment_WhenCalled111_ErrorData()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            var httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == request.Object
            );

            //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            //assign context to controller
            var controller = new ErrorController()
            {
                ControllerContext = controllerContext,
            };

            var mockErrorWork = new Mock<IWebHostEnvironment>();

            mockErrorWork.Setup(m => m.EnvironmentName).Returns(() =>
            {
                return "Development";
            });

            var result = controller.ErrorLocalDevelopment(mockErrorWork.Object);
        }

        [Fact]
        public void ErrorLocalDevelopment_WhenCalled000_ErrorData()
        {
            var controller = new ErrorController
            {
                ControllerContext = new ControllerContext(),

        };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ProblemDetailsFactory = controller.ControllerContext.HttpContext?.RequestServices?.GetRequiredService<ProblemDetailsFactory>();

            //controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            //controller

            controller.HttpContext.Features.Set<IExceptionHandlerFeature>(new ExceptionHandlerFeature
            {
                Error = null
            });

            //  controller.ControllerContext.HttpContext.Features.Get

            //HttpContext.Features.Get<IExceptionHandlerFeature>();

            var mockErrorWork = new Mock<IWebHostEnvironment>();

            mockErrorWork.Setup(m => m.EnvironmentName).Returns(() =>
            {
                return "Development";
            });

            var result = controller.ErrorLocalDevelopment(mockErrorWork.Object);
        }

        //[Fact(Skip = "Do not work!!!", DisplayName = "Error controller")]
        [Fact]
        public void ErrorLocalDevelopment_WhenCalled_ErrorData()
        {
            var mockErrorWork = new Mock<IWebHostEnvironment>();

            mockErrorWork.Setup(m => m.EnvironmentName).Returns(() =>
            {
                return "Development";
            });

            var kkk = new ErrorController();

            var mockErrorController = new Mock<ErrorController>()
            {
                CallBase = true,

            };

            //mockErrorController.Setup(m => m.Problem()).Returns(() =>
            //{
            //    //https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.Core/src/ControllerBase.cs
            //    var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            // HttpContext,
            // statusCode: statusCode ?? 500,
            // title: title,
            // type: type,
            // detail: detail,
            // instance: instance);

            //    return new ObjectResult(problemDetails)
            //    {
            //        StatusCode = problemDetails.Status
            //    };
            //});

            //mockErrorController.Setup(m => m.ErrorLocalDevelopment(mockErrorWork.Object)).Returns(() => {
            //    errorController.
            //});

            var mockfeatureCollection = new Mock<IFeatureCollection>();
            mockfeatureCollection.Setup(m => m.Get<IExceptionHandlerFeature>()).Returns(() =>
           {
               var exceptionHandlerFeature = new ExceptionHandlerFeature
               {
                   Error = new Exception("Exception message")
               };
               return exceptionHandlerFeature;
           });

            mockfeatureCollection.Setup(m => m.Set(It.IsAny<IExceptionHandlerFeature>()));
            mockfeatureCollection.Setup(m => m.IsReadOnly).Returns(true);
            mockfeatureCollection.Setup(m => m.Revision).Returns(1);

            var mockObjectResult = new Mock<ObjectResult>();

            var httpContext = new DefaultHttpContext(mockfeatureCollection.Object);
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);

            kkk.ControllerContext = controllerContext;

            // var problemDetails = ProblemDetailsFactory.CreateProblemDetails(httpContext);
            // var t=  ProblemDetailsFactory.CreateProblemDetails(httpContext);

            // kkk.ProblemDetailsFactory = new

            var hh = mockErrorController.Object;
            var result = mockErrorController.Object.ErrorLocalDevelopment(mockErrorWork.Object) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
        }
    }
}