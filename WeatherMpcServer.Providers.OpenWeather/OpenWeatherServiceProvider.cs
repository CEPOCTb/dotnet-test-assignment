using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Interceptors;
using WeatherMpcServer.Abstraction;
using WeatherMpcServer.Providers.OpenWeather.Models;
using WeatherMpcServer.Providers.OpenWeather.Settings;

namespace WeatherMpcServer.Providers.OpenWeather;

public class OpenWeatherServiceProvider : IWeatherServiceProvider, IDisposable
{
    private readonly IOptionsMonitor<OpenWeatherSettings> _settings;
    private readonly ILogger<OpenWeatherServiceProvider> _logger;
    private readonly IRestClient _restClient;

    public OpenWeatherServiceProvider(IOptionsMonitor<OpenWeatherSettings> settings, ILogger<OpenWeatherServiceProvider> logger)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _restClient = new RestClient(
            options =>
            {
                options.BaseUrl = new Uri("https://api.openweathermap.org/");
                options.Interceptors.Add(
                    new CompatibilityInterceptor()
                    {
                        OnBeforeRequest = message =>
                        {
                            if (_logger.IsEnabled(LogLevel.Debug))
                            {
                                _logger.LogDebug(
                                    "Sending request to OpenWeather API: {Method} {Url}",
                                    message.Method,
                                    message.RequestUri);
                            }

                            return ValueTask.CompletedTask;
                        },
                        OnBeforeDeserialization = response =>
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                if (_logger.IsEnabled(LogLevel.Debug))
                                {
                                    _logger.LogDebug(
                                        "Received response content from OpenWeather API: {Content}",
                                        response.Content);
                                }
                            }
                            else
                            {
                                _logger.LogError("Failed to get response from OpenWeather API: {StatusCode} {ReasonPhrase}, Content: {Content}",
                                    response.StatusCode,
                                    response.ErrorMessage,
                                    response.Content);
                            }
                        }
                    });

            });
    }


    /// <inheritdoc />
    public async ValueTask<string> GetCityWeatherAsync(
        string city,
        string? country = null,
        CancellationToken cancellationToken = default
        )
    {
        var query = country == null ? city : $"{city},{country}";
        var request = new RestRequest($"data/2.5/weather")
            .AddQueryParameter("q", query)
            .AddQueryParameter("appid", _settings.CurrentValue.ApiKey)
            .AddQueryParameter("units", "metric");

        var response = await _restClient.GetAsync<WeatherResponse>(request, cancellationToken);

        if (response == null)
        {
            throw new Exception($"Failed to get weather data for {query}");
        }

        var dateTime = DateTimeOffset.FromUnixTimeSeconds(response.Dt)
            .ToOffset(TimeSpan.FromSeconds(response.Timezone));

        var message =
            $"""
             Weather in {response.Name}, {response.Sys.Country} at {dateTime:g}:
             Condition: {string.Join(", ", response.Weather.Select(weather => $"{weather?.Main} ({weather?.Description})"))}
             Temperature: {response.Main.Temperature:F1}°C (feels like {response.Main.FeelsLike:F1}°C)
             Min/Max: {response.Main.TempMin:F1}°C/{response.Main.TempMax:F1}°C
             Humidity: {response.Main.Humidity}%
             Pressure: {response.Main.Pressure}hPa
             Wind: {response.Wind.Speed:F1}m/s, {response.Wind.Deg}°
             Clouds: {response.Clouds.All}%
             Visibility: {response.Visibility}m
             Sunrise: {DateTimeOffset.FromUnixTimeSeconds(response.Sys.Sunrise).ToOffset(TimeSpan.FromSeconds(response.Timezone)):t}
             Sunset: {DateTimeOffset.FromUnixTimeSeconds(response.Sys.Sunset).ToOffset(TimeSpan.FromSeconds(response.Timezone)):t}
             """;

        _logger.LogDebug("Weather data for {Query}: {Message}", query, message);
        return message;
    }

    /// <inheritdoc />
    public async ValueTask<string[]> GetCityWeatherForecastAsync(
        string city,
        string? country = null,
        int? daysCout = null,
        CancellationToken cancellationToken = default
        )
    {
        var query = country == null ? city : $"{city},{country}";
        var request = new RestRequest($"data/2.5/forecast")
            .AddQueryParameter("q", query)
            .AddQueryParameter("appid", _settings.CurrentValue.ApiKey)
            .AddQueryParameter("units", "metric");

        if (daysCout is > 0)
        {
            // OpenWeather API returns 8 forecasts per day (3-hours intervals), so we multiply by 8 to get the total count
            request.AddQueryParameter("cnt", (daysCout * 8).ToString());
        }

        var response = await _restClient.GetAsync<ForecastResponse>(request, cancellationToken);

        if (response == null)
        {
            throw new Exception($"Failed to get weather forecast data for {query}");
        }

        var messages = response.List
            .Select(forecast =>
            {
                var dateTime = DateTimeOffset.FromUnixTimeSeconds(forecast.Dt)
                    .ToOffset(TimeSpan.FromSeconds(response.City.Timezone));

                return $"""
                       {dateTime:g}:
                       Condition: {string.Join(", ", forecast.Weather.Select(weather => $"{weather?.Main} ({weather?.Description})"))}
                       Temperature: {forecast.Main.Temperature:F1}°C (feels like {forecast.Main.FeelsLike:F1}°C)
                       Min/Max: {forecast.Main.TempMin:F1}°C/{forecast.Main.TempMax:F1}°C
                       Humidity: {forecast.Main.Humidity}%
                       Pressure: {forecast.Main.Pressure}hPa
                       Wind: {forecast.Wind.Speed:F1}m/s, {forecast.Wind.Deg}°
                       Clouds: {forecast.Clouds.All}%
                       Visibility: {forecast.Visibility}m
                       """;
            }).ToArray();

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(
                "Weather forecast data for {Query}: {Messages}",
                query,
                string.Join(";" + Environment.NewLine + Environment.NewLine, messages));
        }

        return messages;
    }

    public void Dispose()
    {
        _restClient.Dispose();
    }
}