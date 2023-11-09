using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_project.Db;
using Second_project.Dto;
using Second_project.Interfaces;
using Second_project.Models.Data;

using System.Reactive.Linq;

namespace Second_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWeatherService _weatherService;
        public WeatherDataController(ApplicationDbContext context, IWeatherService weatherService)
        {
            _context = context;
            _weatherService = weatherService;
        }

        [HttpGet("min-temperatures")]
        public async Task<ActionResult<IEnumerable<WeatherStatsDto>>> GetMinTemperatures()
        {
            var minTemperatures = await _weatherService.GetMinTemperaturesAsync();

            return Ok(minTemperatures);
        }

        [HttpGet("max-wind-speeds")]
        public async Task<ActionResult<IEnumerable<WeatherStatsDto>>> GetMaxWindSpeeds()
        {
            var maxWindSpeeds = await _weatherService.GetMaxWindSpeedByCountryAsync();

            return Ok(maxWindSpeeds);
        }
    }
}
