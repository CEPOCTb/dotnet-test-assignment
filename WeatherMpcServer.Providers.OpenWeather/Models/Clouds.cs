using System.Text.Json.Serialization;

namespace WeatherMpcServer.Providers.OpenWeather.Models;

class Clouds
{
    [JsonPropertyName("all")] public int All { get; set; }
}