namespace BasicMediator.UnitTests.UseCases.ThrowIfNull;

public record ThrowIfNullRequest(string? Value) : IRequest;