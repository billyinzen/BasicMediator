using BasicMediator.IntegrationTests.UseCases.NoHandler;
using BasicMediator.IntegrationTests.UseCases.ReturnIfNotNull;
using BasicMediator.IntegrationTests.UseCases.ThrowIfNull;
using Microsoft.AspNetCore.Mvc;

namespace BasicMediator.IntegrationTests.Controllers;

[Route("/test")]
[ApiController]
public class TestController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetNoHandler(CancellationToken cancellationToken)
    {
        var request = new NoHandlerRequest();
        var result = await mediator.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("void")]
    public async Task<IActionResult> GetVoidNoHandler(CancellationToken cancellationToken)
    {
        var request = new VoidNoHandlerRequest();
        await mediator.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> PostToValueHandler([FromBody] RequestModel model, CancellationToken cancellationToken)
    {
        var request = new ReturnIfNotNullRequest(model.Value);
        var result = await mediator.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("void")]
    public async Task<IActionResult> PostToVoidHandler([FromBody] RequestModel model, CancellationToken cancellationToken)
    {
        var request = new ThrowIfNullRequest(model.Value);
        await mediator.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return NoContent();
    }
}