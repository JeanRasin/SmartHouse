using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartHouse.Infrastructure.Data.Weather.OpenWeather
{
    public class WeatherResponse
    {
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; }
        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }
        [JsonPropertyName("base")]
        public string WeatherBase { get; set; }
        [JsonPropertyName("main")]
        public Main Main { get; set; }
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }
        [JsonPropertyName("wind")]
        public Wind Wind { get; set; }
        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; }
        [JsonPropertyName("dt")]
        public int Dt { get; set; }
        [JsonPropertyName("sys")]
        public Sys Sys { get; set; }
        [JsonPropertyName("timezone")]
        public int TimeZone { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("cod")]
        public int Cod { get; set; }
    }
}
