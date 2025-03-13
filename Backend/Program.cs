var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogger(builder.Configuration);
Log.Information("{ProductName} v{AppInformationalVersion} started.", Aid.ProductName, Aid.AppInformationalVersion);

var hostingConfig = WebHostingOptions.ToModel(builder.Configuration?.GetSection(WebHostingOptions.ConfigName)?.Get<WebHostingOptions>());
builder.WebHost.ConfigWebHost(hostingConfig);

builder.Services.ProvideServices(builder.Configuration);

var app = builder.Build();
await app.Services.WarmUp(builder.Configuration);
app.Prepare();
app.MapEndpoints();
await app.RunAsync();

Log.Information("{ProductName} v{AppInformationalVersion} stopped.", Aid.ProductName, Aid.AppInformationalVersion);
await Log.CloseAndFlushAsync();