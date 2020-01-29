using Bogus;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestBusiness
{
    public class WeatherWorkTest
    {
        [Fact]
        public void Data_Get_Success()
        {
            Randomizer.Seed = new Random(1338);
            var wetaherDataFaker = new Faker<WeatherModel>()
                .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000))
                .RuleFor(o => o.WindDeg, f => f.Random.UShort(0, 360))
                .RuleFor(o => o.Temp, f => f.Random.Float(-100, 100))
                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.Pressure, f => f.Random.Float(0, 1000))
                .RuleFor(o => o.Humidity, f => f.Random.Float(0, 1000))
                .RuleFor(o => o.Description, f => f.Random.Words(5))
                .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000));

            var wetaherData = wetaherDataFaker.Generate();

            var mock = new Mock<IWeatherService>();

            mock.Setup(m => m.GetWeatherAsync()).Returns(async () =>
            {
                return await Task.Run(() =>
                {
                    return wetaherData;
                });
            });

            var weatherWork = new WeatherWork(mock.Object);

            WeatherModel obj = weatherWork.GetWeatherAsync().Result;

            Assert.NotNull(obj);
        }

        [Fact]
        public void Data_Get_Exception()
        {
            var mock = new Mock<IWeatherService>();

            mock.Setup(m => m.GetWeatherAsync()).Returns(() =>
            {
                throw new Exception();
            });

            var weatherWork = new WeatherWork(mock.Object);

            Assert.ThrowsAsync<Exception>(() => weatherWork.GetWeatherAsync());
        }

        /*
        [Fact]
        public void Data_get_httpRequestException_unauthorized_401()
        {
            var mock = new Mock<IWeatherService>();

            mock.Setup(m => m.GetWeather()).Returns(() =>
            {
                throw new HttpRequestException("Response status code does not indicate success: 401 (Unauthorized).");
            });

            var weatherWork = new WeatherWork(mock.Object, 500);

            Assert.Throws<HttpRequestException>(() => weatherWork.GetWeather());
        }
        */

        //[Fact]
        //public void Data_get_exception_counterMax()
        //{
        //    var mock = new Mock<IWeatherService>();

        //    mock.Setup(m => m.GetWeather()).Returns(async () =>
        //    {
        //        return await Task.Run(() =>
        //        {
        //            return new WeatherData
        //            {
        //                Temp = 5,
        //                WindSpeed = 12
        //            };
        //        });
        //    });

        //    var weatherWork = new WeatherWork(mock.Object, 3);

        //    Exception ex = Assert.Throws<Exception>(() =>
        //    {
        //        for (var i = 0; i < 5; i++)
        //        {
        //            weatherWork.GetWeather();
        //        }

        //    });

        //    Assert.Equal("The number of requests per day exceeded.", ex.Message);
        //}

        //[Fact]
        //public void Data_get_httpRequestException_serviceUnavailable_503()
        //{

        //}
    }
}
