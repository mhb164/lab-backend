using Backend.Extensions;
using Backend.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogger(builder.Configuration);
Serilog.Log.Information("{ProductName} v{AppInformationalVersion} started.", Aid.ProductName, Aid.AppInformationalVersion);

var hostingConfig = WebHostingOptions.ToModel(builder.Configuration?.GetSection(WebHostingOptions.ConfigName)?.Get<WebHostingOptions>());
builder.WebHost.ConfigWebHost(hostingConfig);

builder.Services.ProvideServices(builder.Configuration);

var app = builder.Build();
await app.Services.WarmUp(builder.Configuration);
app.Prepare();
app.MapEndpoints();
await app.RunAsync();

Serilog.Log.Information("{ProductName} v{AppInformationalVersion} stopped.", Aid.ProductName, Aid.AppInformationalVersion);
await Serilog.Log.CloseAndFlushAsync();