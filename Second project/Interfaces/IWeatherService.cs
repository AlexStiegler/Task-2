using Newtonsoft.Json;
using Second_project.Dto;
using Second_project.Models.Data;

namespace Second_project.Interfaces
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherData>> FetchWeatherDataAsync();
        Task<IEnumerable<WeatherStatsDto>> GetMaxWindSpeedByCountryAsync();
        Task<IEnumerable<WeatherStatsDto>> GetMinTemperaturesAsync();
    }
}