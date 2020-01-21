namespace SmartHouse.Domain.Core.Weather
{
    public class WeatherModel
    {
        public double WindSpeed { get; set; }
        public double Temp { get; set; }

        public WeatherModel()
        {
          
        }

        public WeatherModel(double windSpeed, double temp)
        {
            WindSpeed = windSpeed;
            Temp = temp;
        }
    }
}
