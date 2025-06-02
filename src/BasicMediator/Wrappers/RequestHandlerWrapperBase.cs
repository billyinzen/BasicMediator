using BasicMediator.Enums;

namespace BasicMediator.Wrappers;

/// <summary>Abstract class defining a generic request handler wrapper with a void response.</summary>
public abstract class RequestHandlerWrapperBase
{
    /// <summary>Handler wrapper for a request object for which the handler does not return a value.</summary>
    /// <param name="request">The request object.</param>
    /// <param name="provider">The service provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public abstract Task<Response> HandleAsync(IRequest request, IServiceProvider provider, CancellationToken cancellationToken);
}

/// <summary>
/// Abstract class defining a generic request handler wrapper which returns <typeparamref name="TResponse"/>.
/// </summary>
public abstract class RequestHandlerWrapperBase<TResponse>
{
    /// <summary>
    /// Handler wrapper for a request object for which the handler returns an instance of
    /// <typeparamref name="TResponse"/>.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="provider">The service provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public abstract Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider provider, CancellationToken cancellationToken);
}