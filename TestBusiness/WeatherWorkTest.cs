using Moq;
using SmartHouse.Business.Data.Weather;
using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
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
            var mock = new Mock<IWeatherService>();

            mock.Setup(m => m.GetWeatherAsync()).Returns(async () =>
            {
                return await Task.Run(() =>
                {
                    return new WeatherData
                    {
                        Temp = 5,
                        WindSpeed = 12
                    };
                });
            });

            var weatherWork = new WeatherWork(mock.Object);

            WeatherData obj = weatherWork.GetWeather();

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

            Assert.Throws<Exception>(() => weatherWork.GetWeather());
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
