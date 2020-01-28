namespace SmartHouse.Domain.Core
{
    public class WeatherModel
    {
        public float WindSpeed { get; set; }
        public ushort WindDeg { get; set; }
        public float Temp { get; set; }
        public string City { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public string Description { get; set; }
        public float FeelsLike { get; set; }

        public WeatherModel()
        {
          
        }

        public WeatherModel(float windSpeed, byte windDeg, float temp, string city, float pressure, float humidity, string description, float feelsLike)
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
