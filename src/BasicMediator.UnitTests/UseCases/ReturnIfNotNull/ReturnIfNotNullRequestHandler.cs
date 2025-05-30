namespace BasicMediator.UnitTests.UseCases.ReturnIfNotNull;

public class ReturnIfNotNullRequestHandler : IRequestHandler<ReturnIfNotNullRequest, string>
{
    public Task<string> HandleAsync(ReturnIfNotNullRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Value, nameof(request.Value));
        return Task.FromResult(request.Value);
    }
}