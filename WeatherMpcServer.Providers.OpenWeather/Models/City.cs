namespace WeatherMpcServer.Providers.OpenWeather.Models;

class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Coord Coord { get; set; } = new();
    public string Country { get; set; } = string.Empty;
    public int Population { get; set; }
    public int Timezone { get; set; }
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
}