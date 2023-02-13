namespace WeatherForecastAPI.Classes
{
    public class LoginCredentials
    {
        public static string login = "opennet";
        public static string password = "1234";

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
