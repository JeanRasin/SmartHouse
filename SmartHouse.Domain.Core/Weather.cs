using Newtonsoft.Json;

namespace SmartHouse.Domain.Core
{
    public class Weather
    {
        [JsonProperty(Required = Required.Always)]
        public float WindSpeed { get; set; }

        [JsonProperty(Required = Required.Always)]
        public ushort WindDeg { get; set; }

        [JsonProperty(Required = Required.Always)]
        public float Temp { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string City { get; set; }

        [JsonProperty(Required = Required.Always)]
        public float Pressure { get; set; }

        [JsonProperty(Required = Required.Always)]
        public float Humidity { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty(Required = Required.Always)]
        public float FeelsLike { get; set; }

        public Weather()
        {
        }

        public Weather(float windSpeed, byte windDeg, float temp, string city, float pressure, float humidity, string description, float feelsLike)
        {
            WindSpeed = windSpeed;
            WindDeg = windDeg;
            Temp = temp;
            City = city;
            Pressure = pressure;
            Humidity = humidity;
            Description = description;
            FeelsLike = feelsLike;
        }
    }
}