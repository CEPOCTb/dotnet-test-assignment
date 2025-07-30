using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WeatherMcpServer.Tools;
using WeatherMpcServer.Providers.OpenWeather.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging
    .AddConfiguration(builder.Configuration)
    .AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace)
    .AddSerilog();

builder.Services.AddSerilog(configuration =>
{
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.File("logs/weather_mcp_server.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 2,
            rollOnFileSizeLimit: true,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
});

// Add the MCP services: the transport to use (stdio) and the tools to register.
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<WeatherTools>();

builder.Services.AddOpenWeatherProvider();

await builder.Build().RunAsync();