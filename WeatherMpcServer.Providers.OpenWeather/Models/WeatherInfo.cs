using System.Text.Json.Serialization;

namespace WeatherMpcServer.Providers.OpenWeather.Models;

class WeatherInfo
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("main")] public string Main { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
    [JsonPropertyName("icon")] public string Icon { get; set; } = string.Empty;
}