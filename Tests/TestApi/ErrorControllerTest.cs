using Bogus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouseAPI.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public class FeatureCollection : IFeatureCollection
        {
            public object this[Type key] { get => new object(); set => throw new NotImplementedException(); }

            public bool IsReadOnly => throw new NotImplementedException();

            public int Revision => throw new NotImplementedException();

            // ... get the required type of feature
            public TFeature Get<TFeature>()
            {
                return (TFeature)this[typeof(TFeature)];    // note: cast here!
            }

            public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public void Set<TFeature>(TFeature instance)
            {
                this[typeof(TFeature)] = instance;          // note!
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

       /* [Fact]
        public void ErrorLocalDevelopment_WhenCalled_ErrorData()
        {
            var mockErrorWork = new Mock<IWebHostEnvironment>();

            mockErrorWork.Setup(m => m.EnvironmentName).Returns(() =>
            {
                return "Development";
            });

             var kkk = new ErrorController();

            var mockErrorController = new Mock<ErrorController>() {
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
                   Error = new Exception("Excaption message")
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
          // var tt=  ProblemDetailsFactory.CreateProblemDetails(httpContext);

         // kkk.ProblemDetailsFactory = new 

            var hh =  mockErrorController.Object;
            var result = mockErrorController.Object.ErrorLocalDevelopment(mockErrorWork.Object) as ObjectResult;

            Assert.IsType<ObjectResult>(result);
        }
        */
    }
}
