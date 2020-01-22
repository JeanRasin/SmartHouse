using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SmartHouse.Service.Weather.OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestAPI
{
    public class OpenWeatherMapServiceTests
    {
        private MockHttpMessageHandler mockHttp;
        private readonly Dictionary<string, string> parm = new Dictionary<string, string>
            {
                { "Url", "https://api.openweathermap.org" },
                { "City", "Perm,ru" },
                { "Api", "f4c946ac33b35d68233bbcf83619eb58" }
            };

        [SetUp]
        public void Setup()
        {
            mockHttp = new MockHttpMessageHandler();
        }

        [Test]
        public void Check_ParamNull_NullReferenceException()
        {
            mockHttp.Fallback.Respond("application/json", "");

            Assert.Throws<NullReferenceException>(() => new OpenWeatherMapService(null, mockHttp));
        }

        [Test]
        public void Check_ParamNot_Exception()
        {
            var parm = new Dictionary<string, string>();

            mockHttp.Fallback.Respond("application/json", "{'name' : 'Test McGee'}");

            var ex = Assert.Throws<Exception>(() => new OpenWeatherMapService(parm, mockHttp));
            Assert.AreEqual("Not parameters.", ex.Message);
        }

        [Test]
        public void Http_Request_WrongResponseJsonException()
        {
            mockHttp.Fallback.Respond("application/json", "{'name' : 'not'}");

            var service = new OpenWeatherMapService(parm, mockHttp);

            Assert.ThrowsAsync<JsonException>(async () => await service.GetWeatherAsync());
        }

        [Test]
        public void Http_Request_Status503()
        {
            mockHttp.Fallback.Respond(HttpStatusCode.ServiceUnavailable);

            //mockHttp.Fallback.Respond(()=>
            //{
            //    throw new HttpRequestException(HttpStatusCode.ServiceUnavailable.ToString(), new WebException("", WebExceptionStatus));
            //});

            var service = new OpenWeatherMapService(parm, mockHttp);

            var ex = Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetWeatherAsync());
            //if (ex.InnerException is WebException webException && webException.Status == WebExceptionStatus.NameResolutionFailure)
            //{
            //    Assert.Pass();
            //}

            Assert.AreEqual("Response status code does not indicate success: 503 (Service Unavailable).", ex.Message);//todo:!!!
        }

        [Test]
        public void Http_Request_Status401()
        {
            mockHttp.Fallback.Respond(HttpStatusCode.Unauthorized);

            var parm = new Dictionary<string, string>
            {
                { "url", "https://api.openweathermap.org" },
                { "city", "Perm,ru" },
                { "api", "0" }
            };

            var service = new OpenWeatherMapService(parm, mockHttp);

            var ex = Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetWeatherAsync());
            Assert.AreEqual("Response status code does not indicate success: 401 (Unauthorized).", ex.Message);
        }

        [Test]
        public void Http_Request_Exception()
        {
            mockHttp.Fallback.Throw(new Exception());

            var service = new OpenWeatherMapService(parm, mockHttp);

            Assert.ThrowsAsync<Exception>(async () => await service.GetWeatherAsync());
        }

        [Test]
        public void Http_RequestException_WriteLogger()
        {
            mockHttp.Fallback.Throw(new Exception());

            var logger = Mock.Of<ILogger<OpenWeatherMapService>>();

            var service = new OpenWeatherMapService(logger, parm, mockHttp);

            Assert.ThrowsAsync<Exception>(async () => await service.GetWeatherAsync());
        }

        [Test]
        public void Http_Request_TimeOut()
        {
            mockHttp.Fallback.Respond(async () =>
            {
                await Task.Delay(2000);
                //System.Threading.Thread.Sleep(2000);// todo:!!!

                return await Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
            });

            var service = new OpenWeatherMapService(parm, mockHttp);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetWeatherAsync());
        }

        [Test]
        public void Http_Request_Success()
        {
            var responseJson = @"{'coord':{'lon':56.29,'lat':58.02},'weather':[{'id':600,'main':'Snow','description':'light snow','icon':'13n'}],'base':'model','main':{'temp':-3.87,'feels_like':-10.16,'temp_min':-3.87,'temp_max':-3.87,'pressure':994,'humidity':91,'sea_level':994,'grnd_level':973},'wind':{'speed':5.24,'deg':228},'snow':{'3h':0.31},'clouds':{'all':100},'dt':1578921976,'sys':{'country':'RU','sunrise':1578891106,'sunset':1578916455},'timezone':18000,'id':511196,'name':'Perm','cod':200}"
                    .Replace('\'', '"');

            // Setup a respond for the user api (including a wildcard in the URL)
            // mockHttp.When(url).Respond("application/json", responseJson); todo:!!!
            mockHttp.Fallback.Respond("application/json", responseJson);

            var service = new OpenWeatherMapService(parm, mockHttp);

            var result = service.GetWeatherAsync().Result;
            Assert.IsNotNull(result);
        }
    }
}