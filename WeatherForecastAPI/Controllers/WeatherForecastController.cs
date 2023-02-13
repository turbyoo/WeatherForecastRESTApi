using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherForecastAPI.Classes;



namespace WeatherForecastAPI.Controllers
{
    [Route("api/WeatherForecastAPI")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {

        [HttpPost("[action]/")]
        public async Task<IActionResult> CityForecast([FromBody] List<string> cities, string login, string password)
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
                    var forecasts = new List<object>();
                    foreach (var city in cities)
                    {
                        var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={ApiKey.Key}&units=metric");
                        response.EnsureSuccessStatusCode();

                        var stringResult = await response.Content.ReadAsStringAsync();
                        var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                        forecasts.Add(new
                        {
                            City = rawWeather.name,
                            Temp = rawWeather.main.temp,
                            Pressure = rawWeather.main.pressure,
                            WindSpeed = rawWeather.wind.speed
                        });
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
            public string speed { get; set; }
        }

        public class Main
        {
            public string temp { get; set; }

            public string pressure { get; set; }

        }
    }
}
