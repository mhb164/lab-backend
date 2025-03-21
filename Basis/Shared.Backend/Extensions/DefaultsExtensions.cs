namespace Shared.Extensions;

public static class DefaultsExtensions
{
    public static IServiceCollection PrepareDefaults(this IServiceCollection services, ILogger? logger, bool addWindowsService = true, bool addCors = true, bool configureJsonOptions = true)
    {
        services.AddSingleton(Aid.AppInfo);

        if (addWindowsService)
        {
            services.AddWindowsService();
            services.AddHostedService<LiveService>();
            logger?.LogInformation("WindowsService support added.");
        }

        if (addCors)
        {
            services.AddCors();
            logger?.LogInformation("Cors added.");
        }

        if (configureJsonOptions)
        {
            // Set the JSON serializer options
            services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = false;
                options.SerializerOptions.PropertyNamingPolicy = null;
                options.SerializerOptions.WriteIndented = true;
                options.SerializerOptions.IncludeFields = false;
            });
            logger?.LogInformation("JsonOptions configured");
        }

        return services;
    }

    public static IApplicationBuilder PrepareDefaults(this IApplicationBuilder app, ILogger? logger,
       bool useCorsAllowAny = true,
       bool useHttpsRedirection = true,
       bool useExceptionMiddleware = true)
    {
        if (useCorsAllowAny)
        {
            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
            logger?.LogInformation("AllowAny-Cors activated.");
        }


        if (useHttpsRedirection)
        {
            // Configure the HTTP request pipeline.1
            app.UseHttpsRedirection();
            logger?.LogInformation("Https redirection activated.");
        }

        if (useExceptionMiddleware)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            logger?.LogInformation("Exception middleware activated.");
        }
        return app;
    }
}
