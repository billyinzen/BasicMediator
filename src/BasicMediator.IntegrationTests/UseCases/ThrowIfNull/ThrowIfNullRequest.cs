namespace BasicMediator.IntegrationTests.UseCases.ThrowIfNull;

public record ThrowIfNullRequest(string? Value) : IRequest;