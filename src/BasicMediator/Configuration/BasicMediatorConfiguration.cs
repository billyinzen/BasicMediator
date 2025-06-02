using BasicMediator.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BasicMediator.Configuration;

/// <summary>Configuration options for the BasicMediator service.</summary>
public class BasicMediatorConfiguration
{
    /// <summary>The assemblies from which to register handlers. Default is `Assembly.GetExecutingAssembly()`.</summary>
    public IEnumerable<Assembly> Assemblies { get; set; } = [Assembly.GetExecutingAssembly()];

    /// <summary>Mediator implementation to register. Default is <see cref="Mediator"/>.</summary>
    public Type MediatorImplementationType { get; set; } = typeof(Mediator);

    /// <summary>
    /// Service lifetime of the mediator service. Default value is <see cref="ServiceLifetime.Transient"/>.
    /// </summary>
    public ServiceLifetime MediatorServiceLifetime { get; set; } = ServiceLifetime.Transient;
}