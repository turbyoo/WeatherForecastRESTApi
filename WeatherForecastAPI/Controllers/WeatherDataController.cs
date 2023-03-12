using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherForecastAPI.Classes;


namespace WeatherForecastAPI.Controllers.WeatherDataController
{
    [Route("api/WeatherForecastAPI")]
    [ApiController]
    public class WeatherDataController : ControllerBase
    {
        private readonly string jsonPath = "WeatherData.json";
        private List<WeatherData> weatherDataList = new List<WeatherData>();

        [HttpPost("[action]/")]
        public async Task<IActionResult> InsertWeatherInfo([FromBody] WeatherData weatherData, string login, string password)
        {
            //Logika do service 
            //

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginCredentials loginCheck = new LoginCredentials();
            if (!loginCheck.IsLoginValid(login, password))
            {
                return Unauthorized();
            }

            try
            {
                List<WeatherData> weatherDataList;

                if (System.IO.File.Exists(jsonPath) && System.IO.File.ReadAllText(jsonPath).Length > 0)
                {
                    string fileContents = System.IO.File.ReadAllText(jsonPath);

                    weatherDataList = JsonConvert.DeserializeObject<List<WeatherData>>(fileContents);

                    var existingForecast = weatherDataList.Where(w => w.City == weatherData.City && w.Date == weatherData.Date).FirstOrDefault();
                    if (existingForecast != null)
                    {
                        return BadRequest("Forecast for this city and date already exists.");
                    }
                }
                else
                {
                    weatherDataList = new List<WeatherData>();
                }

                weatherDataList.Add(weatherData);

                string jsonData = JsonConvert.SerializeObject(weatherDataList);
                System.IO.File.WriteAllText(jsonPath, jsonData);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/")]
        public IActionResult GetWeatherInfo(string city, DateTime date, string login, string password)
        {
            try
            {
                LoginCredentials loginCheck = new LoginCredentials();
                if (!loginCheck.IsLoginValid(login, password))
                {
                    return Unauthorized();
                }

                if (System.IO.File.Exists(jsonPath))
                {
                    string jsonData = System.IO.File.ReadAllText(jsonPath);
                    List<WeatherData> weatherDataList = JsonConvert.DeserializeObject<List<WeatherData>>(jsonData);

                    var weatherInfo = weatherDataList.FirstOrDefault(w => w.City == city && w.Date == date);

                    if (weatherInfo == null)
                    {
                        return NotFound("No weather information found for the specified city and date.");
                    }

                    return Ok(weatherInfo);
                }
                else
                {
                    return NotFound("Weather data file not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
