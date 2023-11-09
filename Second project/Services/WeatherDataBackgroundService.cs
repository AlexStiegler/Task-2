using Microsoft.EntityFrameworkCore;
using Second_project.Db;
using Second_project.Interfaces;

public class WeatherDataFetchingService : BackgroundService
{
    private readonly ILogger<WeatherDataFetchingService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private const int DelayInterval = 60000;

    public WeatherDataFetchingService(ILogger<WeatherDataFetchingService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Weather Data Fetching Service is running.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await FetchAndStoreWeatherData(weatherService, dbContext, stoppingToken);
            }

            await Task.Delay(DelayInterval, stoppingToken);
        }
    }

    private async Task FetchAndStoreWeatherData(IWeatherService weatherService, ApplicationDbContext dbContext, CancellationToken stoppingToken)
    {
        try
        {
            var weatherDataList = await weatherService.FetchWeatherDataAsync();

            foreach (var weatherData in weatherDataList)
            {
                var existingData = await dbContext.WeatherData
                    .FirstOrDefaultAsync(w => w.City == weatherData.City && w.Country == weatherData.Country && w.LastUpdateTime == weatherData.LastUpdateTime, stoppingToken);

                if (existingData != null)
                {
                    dbContext.Update(weatherData);
                }
                else
                {
                    await dbContext.AddAsync(weatherData, stoppingToken);
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching weather data.");
        }
    }
}
