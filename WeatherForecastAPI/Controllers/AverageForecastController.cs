using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherForecastAPI.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Services;
using System.Security.AccessControl;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Drawing;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/WeatherForecastAPI")]
    [ApiController]
    public class AverageForecastController : ControllerBase
    {
        [HttpGet("[action]/")]
        public async Task<IActionResult> ThreeDaysAverageForecast(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");

                    var citySearchResponse = await client.GetAsync($"/data/2.5/weather?q={city}&appid=285737d14e60b6ddc096f14a48c427d1");
                    citySearchResponse.EnsureSuccessStatusCode();

                    var citySearchStringResult = await citySearchResponse.Content.ReadAsStringAsync();
                    var citySearchResult = JsonConvert.DeserializeObject<CitySearchResult>(citySearchStringResult);

                    double lat = citySearchResult.coord.lat;
                    double lon = citySearchResult.coord.lon;

                    // Calculate the Unix timestamps for the last 3 days
                    int currentTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    int threeDaysAgoTimestamp = currentTimestamp - (3 * 24 * 60 * 60);

                    var response = await client.GetAsync($"/data/2.5/onecall?lat={lat}&lon={lon}&start={threeDaysAgoTimestamp}&end={currentTimestamp}&appid=285737d14e60b6ddc096f14a48c427d1");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);

                    // Calculate the average temperature
                    double avgTemp = Math.Round(rawWeather.daily.Take(3).Average(d => d.temp.day)/(24*3),2);

                    string avgTempToString = avgTemp.ToString();

                    // Return the average temperature as a JSON object
                    return Ok(new { temp = avgTempToString });
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

    public class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Daily
    {
        public Temp temp { get; set; }
    }

    public class Temp
    {
        public double day { get; set; }
    }

}
