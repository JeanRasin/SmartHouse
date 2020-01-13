using System.Text.Json.Serialization;

namespace SmartHouse.Infrastructure.Data.Weather.OpenWeather
{
    public class Coord
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }
}
