using System.Text.Json.Serialization;

namespace SmartHouse.Infrastructure.Data.Weather.OpenWeather
{
    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
        [JsonPropertyName("deg")]
        public int Deg { get; set; }
    }
}
