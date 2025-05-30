using BasicMediator.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasicMediator;

/// <summary>Service registration extension methods for the BasicMediator package.</summary>
public static class Startup
{
    /// <summary>Register BasicMediator services.</summary>
    /// <param name="builder">The host application builder.</param>
    /// <returns>A reference to `builder` after the services have been registered.</returns>
    public static IHostApplicationBuilder AddBasicMediator(this IHostApplicationBuilder builder)
        => builder.AddBasicMediator(_ => { });

    /// <summary>Register BasicMediator services with a custom configuration.</summary>
    /// <param name="builder">The host application builder.</param>
    /// <param name="action">The action used to configure the service options</param>
    /// <returns>A reference to `builder` after the services have been registered.</returns>
    public static IHostApplicationBuilder AddBasicMediator(this IHostApplicationBuilder builder,
        Action<BasicMediatorConfiguration> action)
    {
        var configuration = new BasicMediatorConfiguration();
        action.Invoke(configuration);

        builder.Services.AddBasicMediator(configuration);
        return builder;
    }

    internal static IServiceCollection AddBasicMediator(this IServiceCollection services)
        => services.AddBasicMediator(new BasicMediatorConfiguration());

    internal static IServiceCollection AddBasicMediator(this IServiceCollection services,
        BasicMediatorConfiguration configuration)
    {
        // Register the request handlers for the assemblies
        services.RegisterImplementationsOfType(configuration, typeof(IRequestHandler<>));
        services.RegisterImplementationsOfType(configuration, typeof(IRequestHandler<,>));

        // Register the mediator service
        services.Add(new ServiceDescriptor(
            serviceType: typeof(IMediator),
            implementationType: configuration.MediatorImplementationType,
            lifetime: configuration.MediatorServiceLifetime));

        // Return the service collection (for chaining)
        return services;
    }

    private static IServiceCollection RegisterImplementationsOfType(
        this IServiceCollection services,
        BasicMediatorConfiguration configuration,
        Type genericServiceType)
    {
        // Get all the implementations for the generic request handler service type.
        var handlers = configuration.Assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => !t.ContainsGenericParameters)
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericServiceType))
            .ToList();

        foreach (var implementationType in handlers)
        {
            // Get the interface for the handler (IRequestHandler<> or IRequestHandler<,>).
            var serviceType = implementationType.ImplementedInterfaces
                .Single(i => i.GetGenericTypeDefinition() == genericServiceType);

            // Register the handler as a transient service.
            services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
        }

        return services;
    }
}