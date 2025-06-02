using System.Text.Json.Serialization;

namespace BasicMediator.IntegrationTests.Controllers;

public record RequestModel([property: JsonPropertyName("value")] string? Value);