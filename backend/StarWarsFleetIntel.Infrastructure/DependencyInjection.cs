using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Infrastructure.Configuration;
using StarWarsFleetIntel.Infrastructure.Decorators;
using StarWarsFleetIntel.Infrastructure.ExternalServices;
using StarWarsFleetIntel.Infrastructure.PreFlightChecks;
using StarWarsFleetIntel.Infrastructure.Services;

namespace StarWarsFleetIntel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        // Bind configuration settings
        var swapiSettings = configuration
            .GetSection(SwapiSettings.SectionName)
            .Get<SwapiSettings>() ?? new SwapiSettings();

        var cacheSettings = configuration
            .GetSection(CacheSettings.SectionName)
            .Get<CacheSettings>() ?? new CacheSettings();

        // Register settings for DI (optional - if you need to inject these elsewhere)
        services.Configure<SwapiSettings>(configuration.GetSection(SwapiSettings.SectionName));
        services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.SectionName));

        // Add SWAPI Typed Client with Polly
        services.AddHttpClient<ISwapiClient, SwapiClient>(client =>
        {
            client.BaseAddress = new Uri(swapiSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(swapiSettings.TimeoutSeconds);
        })
        .AddTransientHttpErrorPolicy(policy =>
            policy.WaitAndRetryAsync(
                swapiSettings.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
        .AddTransientHttpErrorPolicy(policy =>
            policy.CircuitBreakerAsync(
                swapiSettings.CircuitBreakerFailureThreshold,
                TimeSpan.FromSeconds(swapiSettings.CircuitBreakerDurationSeconds)));

        // Add Memory Cache
        services.AddMemoryCache();

        // Register application services
        services.AddScoped<ICurrencyConverter, CurrencyConverter>();
        services.AddScoped<IStarshipDecoratorFactory, StarshipDecoratorFactory>();

        // Register Chain of Responsibility
        services.AddScoped<IPreFlightCheckHandler>(provider =>
        {
            var crew = new CrewCapacityCheckHandler();
            var hyperdrive = new HyperdriveCheckHandler();
            var consumables = new ConsumablesCheckHandler();

            crew.SetNext(hyperdrive).SetNext(consumables);

            return crew;
        });

        return services;
    }
}
