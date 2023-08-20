using Npgsql;
using System;
using WeatherForecastAPI.Classes;

namespace WeatherForecastAPI.Classes
{
    public class ConnectionModel
    {
        public string server { get; set; }
        public int port { get; set; }
        public string database { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}