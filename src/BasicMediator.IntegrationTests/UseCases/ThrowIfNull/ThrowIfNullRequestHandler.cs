namespace BasicMediator.IntegrationTests.UseCases.ThrowIfNull;

public class ThrowIfNullRequestHandler : IRequestHandler<ThrowIfNullRequest>
{
    public Task HandleAsync(ThrowIfNullRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Value, nameof(request.Value));
        return Task.FromResult(request.Value);
    }
}