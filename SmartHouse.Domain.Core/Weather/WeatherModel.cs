namespace SmartHouse.Domain.Core.Weather
{
    public class WeatherModel
    {
        public int WindSpeed { get; set; }
        public int WindDeg { get; set; }
        public double Temp { get; set; }
        public string City { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; }
        public double FeelsLike { get; set; }

        public WeatherModel()
        {
          
        }

        public WeatherModel(int windSpeed, int windDeg, double temp, string city, int pressure, int humidity, string description, double feelsLike)
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
