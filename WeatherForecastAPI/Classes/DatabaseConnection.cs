using Npgsql;
using System;
using WeatherForecastAPI.Classes;
using Newtonsoft.Json;

namespace WeatherForecastAPI.Classes
{
    public class NpgsqlConnection
    {
        string jsonConfig = File.ReadAllText("appsettings.json");
        AppConfig appConfig = JsonConvert.DeserializeObject<AppConfig>(jsonConfig);
        ConnectionModel connectionModel = appConfig.Connection;

        string connectionString = $"server={connectionModel.server}; port={connectionModel.port}; database={connectionModel.database}; username={connectionModel.username}; password={connectionModel.password}"

        public NpgsqlConnection EstablishConnection()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection to database successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to database: " + ex.Message);
                }

                return conn;
            }
        }
    }
}




