using System.Text.Json.Serialization;

namespace SmartHouse.Service.Weather.OpenWeatherMap.Model
{
    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}
