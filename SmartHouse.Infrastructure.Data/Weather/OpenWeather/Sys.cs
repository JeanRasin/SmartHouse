using System.Text.Json.Serialization;

namespace SmartHouse.Infrastructure.Data.Weather.OpenWeather
{
    public class Sys
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("sunrise")]
        public int Sunrise { get; set; }
        [JsonPropertyName("Sunset")]
        public int Sunset { get; set; }
    }
}
