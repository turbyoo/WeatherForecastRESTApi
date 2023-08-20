using Npgsql;
using WeatherForecastAPI.Classes;

namespace WeatherForecastAPI.Classes
{
    public class LoginCredentials
    {
        private readonly DatabaseConnection _databaseConnection;

        public LoginCredentials(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public bool IsLoginValid(string login, string password)
        {
            NpgsqlConnection connection = _databaseConnection.CreateConnection();

            try
            {
                connection.Open()
                string query = "SELECT password_hash FROM users WHERE username = @login";

                using(NpgsqlCommand command = new NpgsqlCommand(query, connection)) 
                {
                    command.Parameters.AddWithValue("@login", login);
                    using(NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            string storedPasswordHash = reader["password_hash"].ToString();
                            if(storedPasswordHash != password)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            finally 
            { 
                connection.Close();
            }


            return false;
        }
    }
}