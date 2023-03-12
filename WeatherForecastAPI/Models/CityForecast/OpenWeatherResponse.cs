namespace WeatherForecastAPI.Controllers.WeatherForecastController
{

    public class OpenWeatherResponse
    {
        public string name { get; set; }

        public Main main { get; set; }

        public Wind wind { get; set; }
    }
}
