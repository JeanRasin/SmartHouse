using System.Text.Json.Serialization;

namespace SmartHouse.Service.Weather.OpenWeatherMap.Model
{
    public class Wind
    {
        [JsonPropertyName("speed")]
        public float Speed { get; set; }

        [JsonPropertyName("deg")]
        public ushort Deg { get; set; }
    }
}