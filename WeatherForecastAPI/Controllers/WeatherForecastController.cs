using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherForecastAPI.Classes;



namespace WeatherForecastAPI.Controllers
{
    [Route("api/WeatherForecastAPI")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet("[action]/")]
        public async Task<IActionResult> CityForecast(string cities, string login, string password)
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
                    var forecasts = new List<CityWeatherForecast>();
                    foreach (var city in cities.Split(","))
                    {
                        var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={ApiKey.Key}&units=metric");
                        response.EnsureSuccessStatusCode();

                        var stringResult = await response.Content.ReadAsStringAsync();
                        var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                        //Created forecasts list CityWeatherForecast type, then created object cityWeather for each city and added it to the list. (instead of using anonymous types (anonymous object))
                        var cityWeather = new CityWeatherForecast
                        {
                            City = rawWeather.name,
                            Temperature = rawWeather.main.temp,
                            Pressure = rawWeather.main.pressure,
                            WindSpeed = rawWeather.wind.speed
                        };
                        forecasts.Add(cityWeather);
                    }

                    return Ok(forecasts);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }


        public class OpenWeatherResponse
        {
            public string name { get; set; }

            public Main main { get; set; }

            public Wind wind { get; set; }
        }


        public class Wind
        {
            public double speed { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }

            public double pressure { get; set; }

        }

    }
}
