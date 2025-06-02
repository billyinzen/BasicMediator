# Basic Mediator

A super simple mediator pattern implementation.

## Registration and Configuration

This package provides `IHostApplicationBuilder` extension methods to register the BasicMediator services and any 
implemented request handlers.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Without custom configuration
builder.AddBasicMediator();

// With custom configuration action
builder.AddBasicMediator(mediator => 
{
    mediator.Assemblies = [Assembly.GetExecutingAssembly()];
    mediator.MediatorImplementationType = typeof(MyCustomMediator);
    mediator.MediatorServiceLifetime = ServiceLifetime.Singleton;
});
```

### Configuration Options

#### Assemblies

The `Assemblies` option allows the projects containing `IRequestHandler<>` and `IRequestHandler<,>` implementations to 
be specified.

#### Mediator Implementation Type

The `MediatorImplementationType` allows for the definition of a custom implementation of `IMediator` for the service to 
use rather than the in-built implementation.  This can be useful where there is a need for custom routing or extending 
the service functionality.

#### Mediator Service Lifetime

The `MediatorServiceLifetime` option allows for the definition of the service lifetime for the `IMediator` service.  By 
default, this is transient, but custom implementations may need to be scoped differently.

## Requests and Handlers

A request implements `IRequest` or `IRequest<>`, and represents a query or command.  It is generally considered good 
practice to make requests immutable.

**Terminology:** an implementation of `IRequest` which does not have a specified response type (i.e. which is not
`IRequest<>`) is referred to internally as a "Void request".

```csharp
public record MyRequest(string Input) : IRequest<string>

public record MyVoidRequest(string Input) : IRequest
```

Request are routed to the corresponding `IRequestHandler` for the request type:

```csharp
public class MyRequestHandler : IRequestHandler<MyRequest, string>
{
    public Task<string> HandleAsync(MyRequest request, CancellationToken cancellationToken)
        => Task.FromResult(request.Input);
}

public class MyVoidRequestHandler : IRequestHandler<MyVoidRequest>
{
    public Task HandleAsync(MyVoidRequest request, CancellationToken cancellationToken)
        => Task.CompletedTask;
}
```

## Mediator Service

The `IMediator` service describes the `SendAsync` method for both requests and void requests.  When the service is added
to a class, this method is used to send requests:

```csharp
// Context:
public record GetEntitiesQuery(/*...*/) : IRequest<IEnumerable<Entity>>;

// Usage:
public class ExampleController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEntities(GetEntitiesRequestModel model, CancellationToken cancellationToken)
    {
        GetEntitiesQuery request = model.ToQuery();
        var response = await mediator.SendAsync(request, cancellationToken);
        var responseModels = entities.ToDtoCollection();
        return Ok(responseModels);
    }
}
```

## Exceptions

This package defines one exception type - `RequestHandlerException`.  This is thrown in one of two situations:

1. The request passed to the default `IMediator` service does not have a registered handler.
2. It was not possible to instantiate the registered handler for the given request.

The exception message will indicate whether the error is a failure to resolve (1.) or instantiate (2.) the handler. 

## Frequently Asked Questions

### What is this for?

I have several projects use [Mediatr](https://github.com/jbogard/MediatR) to help implement CQRS, but none require, nor 
fully utilise, the functionality it offers.  With that package moving to a commercial license, I created my own little 
CQRS helper - and that became this package.

If you find yourself looking for handler pipelines, broadcast requests, and notifications, please do check out Jimmy's 
package!  I've used similar naming patterns (e.g. `IRequest` and `IRequestHandler` for the interfaces) so swapping over 
shouldn't be very difficult.

If you want to know more about the mediator pattern in general, 
[Refactoring Guru](https://refactoring.guru/design-patterns/mediator) is an easy introduction.  

[Wikipedia](https://en.wikipedia.org/wiki/Command_Query_Responsibility_Segregation) actually has a really simple 
description of CQRS too.