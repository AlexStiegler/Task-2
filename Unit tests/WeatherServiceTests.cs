using Xunit;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Second_project.Db;
using Microsoft.EntityFrameworkCore;
using Second_project.Services;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Microsoft.Data.Sqlite;

public class WeatherServiceTests
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        _context = new ApplicationDbContext(options);

        _httpClient = new HttpClient(new FakeHttpMessageHandler());

        var inMemorySettings = new Dictionary<string, string>
        {
            {"WeatherApiConfigurations:0:Country", "Country1"},
            {"WeatherApiConfigurations:0:Cities:0:Name", "City1"},
            {"WeatherApiConfigurations:0:Cities:0:Url", "http://api.example.com/city1"},
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _weatherService = new WeatherService(configuration, _httpClient, _context);
    }

    [Fact]
    public async Task FetchWeatherDataAsync_ReturnsDataFromApi()
    {

        var result = await _weatherService.FetchWeatherDataAsync();

        result.Should().NotBeEmpty();
        result.First().Temperature.Should().Be(15);
    }
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var fakeApiResponse = JsonConvert.SerializeObject(new
        {
            hourly = new
            {
                time = new[] { "2023-11-09T12:00:00Z" },
                temperature_2m = new[] { 15 },
                cloud_cover = new[] { 75 },
                wind_speed_180m = new[] { 20 }
            }
        });

        return await Task.FromResult(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(fakeApiResponse)
        });
    }
}
