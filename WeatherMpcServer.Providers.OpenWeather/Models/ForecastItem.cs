namespace WeatherMpcServer.Providers.OpenWeather.Models;

class ForecastItem
{
    public long Dt { get; set; }
    public MainInfo Main { get; set; } = new();
    public List<WeatherInfo> Weather { get; set; } = new();
    public Clouds Clouds { get; set; } = new();
    public Wind Wind { get; set; } = new();
    public int Visibility { get; set; }
    public double Pop { get; set; }
    public Rain? Rain { get; set; }
    public Snow? Snow { get; set; }
    public Sys Sys { get; set; } = new();
    public string DtTxt { get; set; } = string.Empty;
}