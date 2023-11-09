using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using Second_project.Interfaces;
using Second_project.Models.Data;
using Second_project.Configs;
using Second_project.Db;
using Second_project.Dto;
using System.Data;

namespace Second_project.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly List<WeatherApiConfig> _apiConfigurations;
        private readonly ApplicationDbContext _context;

        public WeatherService(IConfiguration configuration, HttpClient httpClient, ApplicationDbContext context)
        {
            _apiConfigurations = configuration.GetSection("WeatherApiConfigurations")
                                              .Get<List<WeatherApiConfig>>();
            _httpClient = httpClient;
            _context = context;

        }
        public async Task<IEnumerable<WeatherStatsDto>> GetMaxWindSpeedByCountryAsync()
        {


            var maxWindSpeedByCountryList = new List<WeatherStatsDto>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "dbo.GetMaxWindSpeedByCountry";
                command.CommandType = CommandType.StoredProcedure;

                _context.Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        maxWindSpeedByCountryList.Add(new WeatherStatsDto
                        {
                            Country = result.GetString(result.GetOrdinal("Country")),
                            MaxWindSpeed = result.GetDouble(result.GetOrdinal("MaxWindSpeed")),
                            City =  result.GetString(result.GetOrdinal("City")),
                            LastTimeUpdated = result.GetDateTime(result.GetOrdinal("LastUpdateTime"))
                        });
                    }
                }

                await _context.Database.CloseConnectionAsync();
            }

            return maxWindSpeedByCountryList;
        }
        public async Task<IEnumerable<WeatherStatsDto>> GetMinTemperaturesAsync()
        {
            var weatherStatsList = new List<WeatherStatsDto>();

            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "dbo.GetWeatherStatsWithoutParameters";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command and read the results.
                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        var weatherStat = new WeatherStatsDto
                        {
                            Country = result["Country"].ToString(),
                            MinTemperature = result["MinTemperature"] != DBNull.Value ? (double?)result["MinTemperature"] : null,
                            City = result.GetString(result.GetOrdinal("City")),
                            LastTimeUpdated = result.GetDateTime(result.GetOrdinal("LastUpdateTime"))

                        };

                        weatherStatsList.Add(weatherStat);
                    }
                }
            }

            await connection.CloseAsync();

            return weatherStatsList;
        }

        public async Task<IEnumerable<WeatherData>> FetchWeatherDataAsync()
        {
            var weatherDataList = new List<WeatherData>();

            foreach (var countryConfig in _apiConfigurations)
            {
                foreach (var cityConfig in countryConfig.Cities)
                {
                    var response = await _httpClient.GetAsync(cityConfig.Url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        dynamic responseObject = JsonConvert.DeserializeObject(content);

                        var hourlyData = responseObject.hourly;

                        if (hourlyData != null)
                        {
                            var times = hourlyData.time;
                            var temperatures = hourlyData.temperature_2m;
                            var cloudCover = hourlyData.cloud_cover;
                            var windSpeeds = hourlyData.wind_speed_180m;
                            var weatherData = new WeatherData
                            {
                                Country = countryConfig.Country,
                                City = cityConfig.Name,
                            };
                            if (temperatures.Count > 0)
                            {
                                weatherData.Temperature = temperatures[0];
                            }
                            if (cloudCover.Count > 0)
                            {
                                
                                weatherData.Clouds = cloudCover[0];
                            }
                            if (windSpeeds.Count > 0)
                            {
                                weatherData.WindSpeed = windSpeeds[0];
                            }
                            if (times.Count > 0)
                            {
                                weatherData.LastUpdateTime = times[0];
                            }
                            weatherDataList.Add(weatherData);


                        }

                    }
                    else
                    {
                    }
                }
            }

            return weatherDataList;
        }
    }

}
