using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherForecastAPI.Classes;
//using PostDataAnnotations;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/WeatherForecastAPI")]
    [ApiController]
    public class AverageForecastController : ControllerBase
    {

        [HttpGet("[action]/")]
        public async Task<IActionResult> ThreeDaysAverageForecast(string city, string login, string password)
        {
            using (var client = new HttpClient())
            {
                LoginCredentials loginCheck = new LoginCredentials();
                if (!loginCheck.IsLoginValid(login, password))
                {
                    return Unauthorized();
                }

                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");

                    var citySearchResponse = await client.GetAsync($"/data/2.5/weather?q={city}&appid={ApiKey.Key}");
                    citySearchResponse.EnsureSuccessStatusCode();

                    var citySearchStringResult = await citySearchResponse.Content.ReadAsStringAsync();
                    var citySearchResult = JsonConvert.DeserializeObject<CitySearchResult>(citySearchStringResult);

                    double lat = citySearchResult.coord.lat;
                    double lon = citySearchResult.coord.lon;

                    // Calculate the Unix timestamps for the last 3 days
                    int currentTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    int threeDaysAgoTimestamp = currentTimestamp - (3 * 24 * 60 * 60);

                    var response = await client.GetAsync($"/data/2.5/onecall?lat={lat}&lon={lon}&start={threeDaysAgoTimestamp}&end={currentTimestamp}&appid={ApiKey.Key}");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();


                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);

                    double avgTemp = Math.Round(rawWeather.daily.Take(3).Average(d => d.temp.day) / (24 * 3), 2);
                    var avgPressure = Math.Round(rawWeather.daily.Take(3).Average(d => d.pressure), 0);
                    var avgWindSpeed = Math.Round(rawWeather.daily.Take(3).Average(d => d.wind_speed), 0);

                    var averageWeather = new AverageWeatherForecast
                    {
                        Temp = avgTemp,
                        Pressure = avgPressure,
                        WindSpeed = avgWindSpeed
                    };

                    return Ok(averageWeather);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }

    public class OpenWeatherResponse
    {
        public Daily[] daily { get; set; }

    }

    public class CitySearchResult
    {
        public Coord coord { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }

    }

    public class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Daily
    {
        public Temp temp { get; set; }

        public double pressure { get; set; }

        public double wind_speed { get; set; }
    }

    public class Temp
    {
        public double day { get; set; }
    }

}
