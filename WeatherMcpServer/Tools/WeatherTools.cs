using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using WeatherMpcServer.Abstraction;

namespace WeatherMcpServer.Tools;

public class WeatherTools
{
    private readonly IWeatherServiceProvider _weatherServiceProvider;
    private readonly ILogger<WeatherTools> _logger;

    public WeatherTools(IWeatherServiceProvider weatherServiceProvider, ILogger<WeatherTools> logger)
    {
        _weatherServiceProvider = weatherServiceProvider ?? throw new ArgumentNullException(nameof(weatherServiceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [McpServerTool]
    [Description("Gets current weather conditions for the specified city.")]
    public async Task<string> GetCurrentWeather(
        [Description("The city name to get weather for")] string city,
        [Description("Optional: Country code (e.g., 'US', 'UK')")] string? countryCode = null)
    {

        try
        {
            return await _weatherServiceProvider.GetCityWeatherAsync(city, countryCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching weather data");
            return $"Error fetching weather data: {e.Message}";
        }
    }

    [McpServerTool]
    [Description("Gets weather forecast for the specified city.")]
    public async Task<string[]> GetForecastWeather(
        [Description("The city name to get weather forecast for")] string city,
        [Description("Optional: Country code (e.g., 'US', 'UK')")] string? countryCode = null,
        [Description("Optional: Limit forecast days (max 5 days)")] int? daysCout = null)
    {

        try
        {
            if (daysCout is > 5 or < 1)
            {
                return [$"Days count must be between 1 and 5."];
            }

            return await _weatherServiceProvider.GetCityWeatherForecastAsync(city, countryCode, daysCout);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching forecast data");
            return [$"Error fetching weather data: {e.Message}"];
        }
    }
}