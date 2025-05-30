namespace BasicMediator.UnitTests.UseCases.ReturnIfNotNull;

public record ReturnIfNotNullRequest(string? Value) : IRequest<string>;