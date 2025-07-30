using System.Text.Json.Serialization;

namespace WeatherMpcServer.Providers.OpenWeather.Models;

class WeatherResponse
{
    [JsonPropertyName("coord")] public Coord Coord { get; set; } = new();
    [JsonPropertyName("weather")] public WeatherInfo[] Weather { get; set; } = Array.Empty<WeatherInfo>();
    [JsonPropertyName("base")] public string Base { get; set; } = string.Empty;
    [JsonPropertyName("main")] public MainInfo Main { get; set; } = new();
    [JsonPropertyName("visibility")] public int Visibility { get; set; }
    [JsonPropertyName("wind")] public Wind Wind { get; set; } = new();
    [JsonPropertyName("clouds")] public Clouds Clouds { get; set; } = new();
    [JsonPropertyName("rain")] public Rain? Rain { get; set; }
    [JsonPropertyName("snow")] public Snow? Snow { get; set; }
    [JsonPropertyName("dt")] public long Dt { get; set; }
    [JsonPropertyName("sys")] public Sys Sys { get; set; } = new();
    [JsonPropertyName("timezone")] public int Timezone { get; set; }
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("cod")] public int Cod { get; set; }
}