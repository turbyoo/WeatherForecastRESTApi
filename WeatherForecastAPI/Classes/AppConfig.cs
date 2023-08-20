using Npgsql;
using System.IO;

namespace WeatherForecastAPI.Classes
{
    public class AppConfig
    {
        public ConnectionModel Connection {  get; set; }
    }
}
