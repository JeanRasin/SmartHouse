using Bogus;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BusinessTest
{
    [Collection("Weather work")]
    public class WeatherWorkTest
    {
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly Mock<IDatabase> _mockDatabase;

        public WeatherWorkTest()
        {
            _mockWeatherService = new Mock<IWeatherService>();

            _mockDatabase = new Mock<IDatabase>();
        }

        #region WeatherWork

        /// <summary>
        /// Get weather cache.
        /// </summary>
        [Fact]
        [Trait("GetWeatherAsync", "Success")]
        public async void GetWeatherAsync_WeatherCache()
        {
            // Arrange
            // Random constant.
            Randomizer.Seed = new Random(1338);

            async static Task<RedisValue> MoqStringGetAsync()
            {
                Weather data = new Faker<Weather>()
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

                string dataJson = JsonSerializer.Serialize(data);

                var result = new RedisValue(dataJson);

                return await Task.Run(() => result);
            }

            _mockDatabase.Setup(_ => _.StringGetAsync(
                It.Is<RedisKey>(_ => _ == new RedisKey("GetWeather")),
                It.IsAny<CommandFlags>()))
                .Returns(MoqStringGetAsync());

            var weatherWork = new WeatherWork(
                weatherService: _mockWeatherService.Object,
                _mockDatabase.Object,
                new TimeSpan(1, 0, 0));

            // Act
            Weather weather = await weatherWork.GetWeatherAsync();

            // Assert
            Assert.NotNull(weather);
        }

        /// <summary>
        /// Get weather.
        /// </summary>
        [Fact]
        [Trait("GetWeatherAsync", "Success")]
        public async void GetWeatherAsync_Weather()
        {
            // Arrange
            // Random constant.
            Randomizer.Seed = new Random(1338);

            async static Task<Weather> MoqGetWeatherAsync(CancellationTokenSource tokenSource, int sec)
            {
                CancellationToken token = tokenSource.Token;

                await Task.Delay(sec * 1000);

                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                return await Task.Run(() =>
                {
                    Weather wetaherData = new Faker<Weather>()
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

            _mockWeatherService.Setup(_ => _.GetWeatherAsync(
                It.IsAny<CancellationToken>()))
                .Returns(MoqGetWeatherAsync(tokenSource: tokenSource, sec: 2));

            var weatherWork = new WeatherWork(
                weatherService: _mockWeatherService.Object,
                _mockDatabase.Object,
                new TimeSpan(1, 0, 0),
                timeOutSec: 4);

            _mockDatabase.Setup(_ => _.StringSetAsync(
                It.Is<RedisKey>(_ => _ == new RedisKey("GetWeather")),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()
                )).Verifiable();

            // Act
            Weather weather = await weatherWork.GetWeatherAsync(tokenSource);

            // Assert
            Assert.NotNull(weather);

            _mockDatabase.Verify(v => v.StringSetAsync(
             It.Is<RedisKey>(_ => _ == new RedisKey("GetWeather")),
             It.IsAny<RedisValue>(),
             It.IsAny<TimeSpan>(),
             It.IsAny<When>(),
             It.IsAny<CommandFlags>()),
             Times.Once);
        }

        /// <summary>
        /// When getting weather get an exception.
        /// </summary>
        [Fact]
        [Trait("GetWeatherAsync", "Exception")]
        public void GetWeatherAsync_Exception()
        {
            // Arrange
            _mockWeatherService.Setup(_ => _.GetWeatherAsync(
                It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var weatherWork = new WeatherWork(_mockWeatherService.Object,
                _mockDatabase.Object,
                new TimeSpan(1, 0, 0));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(weatherWork.GetWeatherAsync);
        }

        /// <summary>
        /// When receiving weather, get a timeout exception.
        /// </summary>
        [Fact]
        [Trait("GetWeatherAsync", "OperationCanceledException")]
        public async void GetWeatherAsync_TimeOut_OperationCanceledException()
        {
            // Arrange
            async static Task<Weather> MoqGetWeatherAsync(CancellationTokenSource tokenSource, int sec)
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

            var tokenSource = new CancellationTokenSource();

            _mockWeatherService.Setup(_ => _.GetWeatherAsync(
                It.IsAny<CancellationToken>()))
                .Returns(MoqGetWeatherAsync(tokenSource: tokenSource, sec: 3));

            var weatherWork = new WeatherWork(
                weatherService: _mockWeatherService.Object,
                _mockDatabase.Object,
                new TimeSpan(1, 0, 0),
                timeOutSec: 2);

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => weatherWork.GetWeatherAsync(tokenSource));
        }

        #endregion WeatherWork
    }
}