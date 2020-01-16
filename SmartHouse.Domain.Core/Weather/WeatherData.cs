namespace SmartHouse.Domain.Core.Weather
{
    public class WeatherData
    {
        public double WindSpeed { get; set; }
        public double Temp { get; set; }

        public WeatherData()
        {
          
        }

        public WeatherData(double windSpeed, double temp)
        {
            WindSpeed = windSpeed;
            Temp = temp;
        }
    }
}
