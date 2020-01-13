using System.Text.Json.Serialization;

namespace SmartHouse.Infrastructure.Data.Weather.OpenWeather
{
    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}
