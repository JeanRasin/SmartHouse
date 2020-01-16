using System.Text.Json.Serialization;

namespace SmartHouse.Service.Weather.OpenWeatherMap.Model
{
    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
        [JsonPropertyName("deg")]
        public int Deg { get; set; }
    }
}
