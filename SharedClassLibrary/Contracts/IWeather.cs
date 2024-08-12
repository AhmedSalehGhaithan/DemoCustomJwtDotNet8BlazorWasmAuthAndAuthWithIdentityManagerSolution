
using SharedClassLibrary.DTOs;

namespace SharedClassLibrary.Contracts
{
    public interface IWeather
    {
        Task<WeatherForecast[]> GetWeatherForecast();
    }
}
