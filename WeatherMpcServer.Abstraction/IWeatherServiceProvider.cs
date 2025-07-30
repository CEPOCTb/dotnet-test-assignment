namespace WeatherMpcServer.Abstraction;

public interface IWeatherServiceProvider
{
    /// <summary>
    /// Gets the current weather for a specified city.
    /// </summary>
    /// <param name="city">The name of the city.</param>
    /// <param name="country">Optional two-letter country code (e.g., 'US', 'UK') to disambiguate cities with the same name.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests during asynchronous operations.</param>
    /// <returns>A string describing the current weather in the specified city.</returns>
    ValueTask<string> GetCityWeatherAsync(string city, string? country = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the weather forecast for a specified city.
    /// </summary>
    /// <param name="city">The name of the city.</param>
    /// <param name="country">Optional two-letter country code (e.g., 'US', 'UK') to disambiguate cities with the same name.</param>
    /// <param name="daysCout">Optional number of days to forecast. If not specified, returns default forecast period.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests during asynchronous operations.</param>
    /// <returns>An array of strings, each describing the weather forecast for a 3-hours period of day in the specified city.</returns>
    ValueTask<string[]> GetCityWeatherForecastAsync(string city, string? country = null, int? daysCout = null, CancellationToken cancellationToken = default);
}