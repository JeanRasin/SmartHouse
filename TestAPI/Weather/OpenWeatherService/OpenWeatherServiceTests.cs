using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SmartHouse.Infrastructure.Data.Weather;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestAPI
{
    public class OpenWeatherServiceTests
    {
        private MockHttpMessageHandler mockHttp;
        //  private const string url = "https://api.openweathermap.org/*";
        private readonly Dictionary<string, string> parm = new Dictionary<string, string>
            {
                { "city", "Perm,ru" },
                { "api", "f4c946ac33b35d68233bbcf83619eb58" }
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

            Assert.Throws<NullReferenceException>(() => new OpenWeatherService(null, mockHttp));
        }

        [Test]
        public void Check_ParamNot_Exception()
        {
            var parm = new Dictionary<string, string>();

            mockHttp.Fallback.Respond("application/json", "{'name' : 'Test McGee'}");

            var ex = Assert.Throws<Exception>(() => new OpenWeatherService(parm, mockHttp));
            Assert.AreEqual("Not parameters.", ex.Message);
        }

        [Test]
        public void Http_Request_WrongResponseJsonException()
        {
            mockHttp.Fallback.Respond("application/json", "{'name' : 'not'}");

            var ows = new OpenWeatherService(parm, mockHttp);

            Assert.ThrowsAsync<JsonException>(async () => await ows.GetWeather());
        }

        [Test]
        public void Http_Request_Status503()
        {
            mockHttp.Fallback.Respond(HttpStatusCode.ServiceUnavailable);

            //mockHttp.Fallback.Respond(()=>
            //{
            //    throw new HttpRequestException(HttpStatusCode.ServiceUnavailable.ToString(), new WebException("", WebExceptionStatus));
            //});

            var ows = new OpenWeatherService(parm, mockHttp);

            var ex = Assert.ThrowsAsync<HttpRequestException>(async () => await ows.GetWeather());
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
                { "city", "Perm,ru" },
                { "api", "0" }
            };

            var ows = new OpenWeatherService(parm, mockHttp);

            var ex = Assert.ThrowsAsync<HttpRequestException>(async () => await ows.GetWeather());
            Assert.AreEqual("Response status code does not indicate success: 401 (Unauthorized).", ex.Message);
        }

        [Test]
        public void Http_Request_Exception()
        {
            mockHttp.Fallback.Throw(new Exception());

            var ows = new OpenWeatherService(parm, mockHttp);

            Assert.ThrowsAsync<Exception>(async () => await ows.GetWeather());
        }

        [Test]
        public void Http_RequestException_WriteLogger()
        {
            mockHttp.Fallback.Throw(new Exception());

            var logger = Mock.Of<ILogger<OpenWeatherService>>();

            var ows = new OpenWeatherService(logger, parm, mockHttp);

            Assert.ThrowsAsync<Exception>(async () => await ows.GetWeather());
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

            var ows = new OpenWeatherService(parm, mockHttp);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await ows.GetWeather());
        }

        [Test]
        public void Http_Request_Success()
        {
            var responseJson = @"{'coord':{'lon':56.29,'lat':58.02},'weather':[{'id':600,'main':'Snow','description':'light snow','icon':'13n'}],'base':'model','main':{'temp':-3.87,'feels_like':-10.16,'temp_min':-3.87,'temp_max':-3.87,'pressure':994,'humidity':91,'sea_level':994,'grnd_level':973},'wind':{'speed':5.24,'deg':228},'snow':{'3h':0.31},'clouds':{'all':100},'dt':1578921976,'sys':{'country':'RU','sunrise':1578891106,'sunset':1578916455},'timezone':18000,'id':511196,'name':'Perm','cod':200}"
                    .Replace('\'', '"');

            // Setup a respond for the user api (including a wildcard in the URL)
            // mockHttp.When(url).Respond("application/json", responseJson); todo:!!!
            mockHttp.Fallback.Respond("application/json", responseJson);

            var ows = new OpenWeatherService(parm, mockHttp);

            var result = ows.GetWeather().Result;
            Assert.IsNotNull(result);
        }
    }
}