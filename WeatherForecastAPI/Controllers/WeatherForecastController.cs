using Microsoft.AspNetCore.Mvc;
using WeatherForecastAPI.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Services;
using System.Linq.Expressions;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/WeatherForecastAPI")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        
        [HttpPost("[action]/")]
        public async Task<IActionResult> CityForecast([FromBody] List<string> cities)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var forecasts = new List<object>();
                    foreach (var city in cities)
                    {
                        var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=285737d14e60b6ddc096f14a48c427d1&units=metric");
                        response.EnsureSuccessStatusCode();

                        var stringResult = await response.Content.ReadAsStringAsync();
                        var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                        forecasts.Add(new
                        {
                            City = rawWeather.Name,
                            Temp = rawWeather.Main.Temp,
                            Pressure = rawWeather.Main.Pressure,
                            WindSpeed = rawWeather.Wind.Speed
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
            public string Name { get; set; }

            public Main Main { get; set; }

            public Wind Wind { get; set; }
        }


        public class Wind
        {
            public string Speed { get; set; } 
        }

        public class Main
        {
            public string Temp { get; set; }

            public string Pressure { get; set; }

        }
    }
}
