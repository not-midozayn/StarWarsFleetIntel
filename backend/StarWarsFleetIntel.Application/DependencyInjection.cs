using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace StarWarsFleetIntel.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        // Add AutoMapper
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

        // Add FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
