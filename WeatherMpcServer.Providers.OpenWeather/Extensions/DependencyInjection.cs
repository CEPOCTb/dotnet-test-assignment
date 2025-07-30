using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMpcServer.Abstraction;
using WeatherMpcServer.Providers.OpenWeather.Settings;

namespace WeatherMpcServer.Providers.OpenWeather.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddOpenWeatherProvider(this IServiceCollection services)
    {
        services.AddOptions<OpenWeatherSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("OpenWeather").Bind(settings);
            })
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.ApiKey), "OpenWeather API key is required.")
            .ValidateOnStart();

        services.AddSingleton<IWeatherServiceProvider, OpenWeatherServiceProvider>();

        return services;
    }
}