namespace WeatherForecastAPI.Classes
{
    public class LoginCredentials
    {
        private const string login = "opennet";
        private const string password = "1234";

        public bool IsLoginValid(string login, string password)
        {
            if (login != LoginCredentials.login || password != LoginCredentials.password)
            {
                return false;
            }
            return true; ;
        }
    }
}
