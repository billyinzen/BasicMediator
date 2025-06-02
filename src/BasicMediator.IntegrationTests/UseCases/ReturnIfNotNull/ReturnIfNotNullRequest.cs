namespace BasicMediator.IntegrationTests.UseCases.ReturnIfNotNull;

public record ReturnIfNotNullRequest(string? Value) : IRequest<string>;