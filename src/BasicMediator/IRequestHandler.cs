namespace BasicMediator;

/// <summary>Describes the handler for an instance of <typeparamref name="TRequest"/>.</summary>
/// <typeparam name="TRequest">The type of request handled.</typeparam>
public interface IRequestHandler<in TRequest>
    : IRequestHandlerBase<TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Handler for an instance of <typeparamref name="TRequest"/> which does not return a value.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>Describes the handler for an instance of <typeparamref name="TRequest"/>.</summary>
/// <typeparam name="TRequest">The type of request handled.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    : IRequestHandlerBase<TRequest>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handler for an instance of <typeparamref name="TRequest"/> which returns a <typeparamref name="TResponse"/>
    /// value.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>Generic marker for a request handler type.</summary>
public interface IRequestHandlerBase<in TRequest>
    where TRequest : IRequestBase;