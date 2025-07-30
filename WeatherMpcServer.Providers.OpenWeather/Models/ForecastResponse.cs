namespace WeatherMpcServer.Providers.OpenWeather.Models;

class ForecastResponse
{
    public List<ForecastItem> List { get; set; } = new();
    public City City { get; set; } = new();
}