using System.Text.Json.Serialization;

namespace WeatherMpcServer.Providers.OpenWeather.Models;

class Sys
{
    [JsonPropertyName("type")] public int Type { get; set; }
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("country")] public string Country { get; set; } = string.Empty;
    [JsonPropertyName("sunrise")] public long Sunrise { get; set; }
    [JsonPropertyName("sunset")] public long Sunset { get; set; }
}