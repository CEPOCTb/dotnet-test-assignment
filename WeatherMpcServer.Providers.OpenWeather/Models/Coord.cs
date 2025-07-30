using System.Text.Json.Serialization;

namespace WeatherMpcServer.Providers.OpenWeather.Models;

class Coord
{
    [JsonPropertyName("lon")] public float Lon { get; set; }
    [JsonPropertyName("lat")] public float Lat { get; set; }
}