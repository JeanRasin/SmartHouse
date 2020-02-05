using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SmartHouse.Service.Weather.OpenWeatherMap;
using SmartHouse.Service.Weather.OpenWeatherMap.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


// ≈сть проблемы с токеном из-за потери кода.

namespace TestService
{
    public class OpenWeatherMapServiceTests
    {
        private MockHttpMessageHandler mockHttp;
        private readonly Dictionary<string, string> parm = new Dictionary<string, string>
            {
                { "url", "https://api.openweathermap.org" },
                { "city", "Perm,ru" },
                { "api", "f4c946ac33b35d68233bbcf83619eb58" }
            };

        [SetUp]
        public void Setup()
        {
            mockHttp = new MockHttpMessageHandler();

            // Random constant.
            Randomizer.Seed = new Random(1338);
        }

        /// <summary>
        /// Generate random data.
        /// </summary>
        /// <returns></returns>
        private static WeatherResponse GetWeatherResponse()
        {
            var coord = new Faker<Coord>()
                .RuleFor(o => o.Lon, f => f.Random.Float(0, 1000))
                .RuleFor(o => o.Lat, f => f.Random.Float(0, 1000));

            var weather = new Faker<Weather>()
                .RuleFor(o => o.Id, f => f.Random.Number(int.MaxValue))
                 .RuleFor(o => o.Main, f => f.Random.Word())
                  .RuleFor(o => o.Description, f => f.Random.Word())
                   .RuleFor(o => o.Icon, f => f.Random.Word());

            var main = new Faker<Main>()
                 .RuleFor(o => o.Temp, f => f.Random.Float(-30, 30))
                 .RuleFor(o => o.FeelsLike, f => f.Random.Float(-30, 30))
                 .RuleFor(o => o.TempMin, f => f.Random.Float(-30, 30))
                 .RuleFor(o => o.TempMax, f => f.Random.Float(-30, 30))
                  .RuleFor(o => o.Pressure, f => f.Random.Float(700, 750))
                   .RuleFor(o => o.Humidity, f => f.Random.Float(10, 99));

            var wind = new Faker<Wind>()
                 .RuleFor(o => o.Speed, f => f.Random.Float(0, 15))
                 .RuleFor(o => o.Deg, f => f.Random.UShort(0, 360));

            var clouds = new Faker<Clouds>()
                .RuleFor(o => o.All, f => f.Random.Number(1));

            var sys = new Faker<Sys>()
                  .RuleFor(o => o.Type, f => f.Random.Number(1))
                  .RuleFor(o => o.Id, f => f.Random.Number(int.MaxValue))
                  .RuleFor(o => o.Country, f => f.Address.Country())
                  .RuleFor(o => o.Sunrise, f => f.Random.Number(int.MaxValue))
                  .RuleFor(o => o.Sunset, f => f.Random.Number(int.MaxValue));

            WeatherResponse wetaherData = new Faker<WeatherResponse>()
                  .RuleFor(o => o.Coord, f => coord.Generate())
                   .RuleFor(o => o.Weather, f => weather.Generate(3))
                 .RuleFor(o => o.WeatherBase, f => f.Random.Word())
                   .RuleFor(o => o.Main, f => main.Generate())
                  .RuleFor(o => o.Visibility, f => f.Random.Number(10))
                  .RuleFor(o => o.Wind, f => wind.Generate())
                  .RuleFor(o => o.Clouds, f => clouds.Generate())
                  .RuleFor(o => o.Dt, f => f.Random.Number(int.MaxValue))
                   .RuleFor(o => o.Sys, f => sys.Generate())
                   .RuleFor(o => o.TimeZone, f => f.Random.Number(int.MaxValue))
                    .RuleFor(o => o.Id, f => f.Random.Number(int.MaxValue))
                    .RuleFor(o => o.Name, f => f.Address.City())
                    .RuleFor(o => o.Cod, f => f.Random.Number(int.MaxValue))
                  .Generate();
            return wetaherData;
        }

        /// <summary>
        /// Parameters is null.
        /// </summary>
        [Test]
        public void Check_ParamNull_NullReferenceException()
        {
            mockHttp.Fallback.Respond("application/json", "");

            Assert.Throws<ArgumentNullException>(() => new OpenWeatherMapService(null, mockHttp));
        }

        /// <summary>
        /// No parameters.
        /// </summary>
        [Test]
        public void Check_ParamNot_Exception()
        {
            mockHttp.Fallback.Respond("application/json", "{'name' : 'Test McGee'}");

            var ex = Assert.Throws<Exception>(() => new OpenWeatherMapService(new Dictionary<string, string>(), mockHttp));
            Assert.AreEqual("Not parameters.", ex.Message);
        }

        /// <summary>
        /// Incorrect answer. Error json conversion.
        /// </summary>
        [Test]
        public void Http_Request_WrongResponseJsonException()
        {
            mockHttp.Fallback.Respond("application/json", "{'name' : 'not'}");

            var service = new OpenWeatherMapService(parm, mockHttp);

            Assert.ThrowsAsync<JsonException>(service.GetWeatherAsync);
        }

        /// <summary>
        /// Server error 503. Repeat requests for a certain time then cause interruption with a token. !!!
        /// </summary>
        [Test]
        public void Http_Request_Status503()
        {
            mockHttp.Fallback.Respond(HttpStatusCode.ServiceUnavailable);

            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                tokenSource.Cancel();
            });

            var service = new OpenWeatherMapService(parm, mockHttp);

            var ex = Assert.ThrowsAsync<HttpRequestException>(() => service.GetWeatherAsync(token));

            Assert.AreEqual("Response status code does not indicate success: 503 (Service Unavailable).", ex.Message);
        }

        /// <summary>
        /// Server error 401. Repeat requests for a certain time then cause interruption with a token. !!!
        /// </summary>
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

            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                tokenSource.Cancel();
            });

            var service = new OpenWeatherMapService(parm, mockHttp);

            var ex = Assert.ThrowsAsync<HttpRequestException>(() => service.GetWeatherAsync(token));
            Assert.AreEqual("Response status code does not indicate success: 401 (Unauthorized).", ex.Message);
        }

        /// <summary>
        /// Raise on exception.
        /// </summary>
        [Test]
        public void Http_Request_Exception()
        {
            mockHttp.Fallback.Throw(new Exception());

            var service = new OpenWeatherMapService(parm, mockHttp);

            Assert.ThrowsAsync<Exception>(service.GetWeatherAsync);
        }

        /// <summary>
        /// Write log.
        /// </summary>
        [Test]
        public void Http_RequestException_WriteLogger()
        {
            mockHttp.Fallback.Throw(new Exception());

            var logger = Mock.Of<ILogger<OpenWeatherMapService>>();

            var service = new OpenWeatherMapService(parm, handler: mockHttp, logger: logger);

            Assert.ThrowsAsync<Exception>(service.GetWeatherAsync);
        }

        /// <summary>
        /// !!!
        /// </summary>
        [Test]
        public void Http_Request_TimeOut()
        {
            mockHttp.Fallback.Respond(async () =>
            {
                await Task.Delay(2000);

                return await Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
            });

            var service = new OpenWeatherMapService(parm, mockHttp);

            Assert.ThrowsAsync<InvalidOperationException>(service.GetWeatherAsync);
        }

        /// <summary>
        /// Successful request.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Http_Request_Success()
        {
            WeatherResponse wetaherData = GetWeatherResponse();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var responseJson = JsonSerializer.Serialize(wetaherData, serializeOptions);

            mockHttp.Fallback.Respond("application/json", responseJson);

            var service = new OpenWeatherMapService(parm, mockHttp);

            var result = await service.GetWeatherAsync();
            Assert.IsNotNull(result);
        }
    }
}