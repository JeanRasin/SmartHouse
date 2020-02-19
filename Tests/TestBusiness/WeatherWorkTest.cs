using Bogus;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestBusiness
{
    [CollectionDefinition("Weather work")]
    public class WeatherWorkTest
    {
        #region WeatherWork
        [Fact]
        public async void GetWeatherAsync_WeatherModel()
        {
            Randomizer.Seed = new Random(1338);

            async static Task<WeatherModel> MoqGetWeatherAsync(CancellationTokenSource tokenSource, int sec)
            {
                CancellationToken token = tokenSource.Token;

                await Task.Delay(sec * 1000);

                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                return await Task.Run(() =>
                {
                    WeatherModel wetaherData = new Faker<WeatherModel>()
                    .StrictMode(false)
                    .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000))
                    .RuleFor(o => o.WindDeg, f => f.Random.UShort(0, 360))
                    .RuleFor(o => o.Temp, f => f.Random.Float(-100, 100))
                    .RuleFor(o => o.City, f => f.Address.City())
                    .RuleFor(o => o.Pressure, f => f.Random.Float(0, 1000))
                    .RuleFor(o => o.Humidity, f => f.Random.Float(0, 1000))
                    .RuleFor(o => o.Description, f => f.Random.Words(5))
                    .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000))
                    .Generate();

                    return wetaherData;
                });
            }

            var tokenSource = new CancellationTokenSource();
            var mock = new Mock<IWeatherService>();

            mock.Setup(m => m.GetWeatherAsync(It.IsAny<CancellationToken>())).Returns(MoqGetWeatherAsync(tokenSource: tokenSource, sec: 2));

            var weatherWork = new WeatherWork(weatherService: mock.Object, timeOutSec: 4);

            WeatherModel weather = await weatherWork.GetWeatherAsync(tokenSource);

            Assert.NotNull(weather);
        }

        [Fact]
        public void GetWeatherAsync_Exception()
        {
            var mock = new Mock<IWeatherService>();

            mock.Setup(m => m.GetWeatherAsync(It.IsAny<CancellationToken>())).Throws(new Exception());

            var weatherWork = new WeatherWork(mock.Object);

            Assert.ThrowsAsync<Exception>(weatherWork.GetWeatherAsync);
        }

        [Fact]
        public async void GetWeatherAsync_TimeOut_OperationCanceledException()
        {
            async static Task<WeatherModel> MoqGetWeatherAsync(CancellationTokenSource tokenSource, int sec)
            {
                CancellationToken token = tokenSource.Token;

                while (true)
                {
                    await Task.Delay(sec * 1000);

                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }
                }
            }

            var mock = new Mock<IWeatherService>();
            var tokenSource = new CancellationTokenSource();

            mock.Setup(m => m.GetWeatherAsync(It.IsAny<CancellationToken>())).Returns(MoqGetWeatherAsync(tokenSource: tokenSource, sec: 3));

            var weatherWork = new WeatherWork(weatherService: mock.Object, timeOutSec: 2);

            await Assert.ThrowsAsync<OperationCanceledException>(() => weatherWork.GetWeatherAsync(tokenSource));
        }
        #endregion
    }
}
