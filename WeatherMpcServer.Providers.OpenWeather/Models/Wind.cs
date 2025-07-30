using System.Text.Json.Serialization;

namespace WeatherMpcServer.Providers.OpenWeather.Models;

class Wind
{
    [JsonPropertyName("speed")] public float Speed { get; set; }
    [JsonPropertyName("deg")] public int Deg { get; set; }
    [JsonPropertyName("gust")] public float Gust { get; set; }
}