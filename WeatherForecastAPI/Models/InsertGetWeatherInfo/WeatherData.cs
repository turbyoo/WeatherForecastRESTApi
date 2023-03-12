namespace WeatherForecastAPI.Controllers.WeatherDataController
{
    public class WeatherData
    {
        public string City { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
    }
}
