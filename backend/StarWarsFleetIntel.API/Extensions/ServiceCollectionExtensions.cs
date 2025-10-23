using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using StarWarsFleetIntel.API.Configuration;
using StarWarsFleetIntel.API.Middleware;

namespace StarWarsFleetIntel.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind CORS settings
            var corsSettings = configuration
                .GetSection(CorsSettings.SectionName)
                .Get<CorsSettings>() ?? new CorsSettings();

            // Register settings for DI (optional)
            services.Configure<CorsSettings>(configuration.GetSection(CorsSettings.SectionName));

            // Add OpenTelemetry
            services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .AddSource("StarWarsFleetIntel")
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService("StarWarsFleetIntel.API"))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter();
                });

            // Add CORS with configured origins
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontends", policy =>
                {
                    policy.WithOrigins(corsSettings.AllowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }

}
