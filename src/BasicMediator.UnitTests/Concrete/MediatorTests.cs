using BasicMediator.Concrete;
using BasicMediator.Exceptions;
using BasicMediator.UnitTests.UseCases.ReturnIfNotNull;
using BasicMediator.UnitTests.UseCases.ThrowIfNull;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BasicMediator.UnitTests.Concrete;

public static class MediatorTests
{
    public class SendAsync
    {
        private static Mediator CreateSut(IServiceProvider serviceProvider) => new(serviceProvider);

        [Fact]
        public async Task ShouldThrowException_WhenHandlerNotFound_ForVoidRequest()
        {
            var provider = new ServiceCollection().BuildServiceProvider();
            var sut = CreateSut(provider);

            await Assert.ThrowsAsync<RequestHandlerException>(()
                => sut.SendAsync(new ThrowIfNullRequest("has value"), CancellationToken.None));
        }

        [Fact]
        public async Task ShouldInvokeHandler_WhenHandlerFound_ForVoidRequest()
        {
            var handler = Substitute.For<IRequestHandler<ThrowIfNullRequest>>();
            var provider = new ServiceCollection()
                .AddTransient<IRequestHandler<ThrowIfNullRequest>>(_ => handler)
                .BuildServiceProvider();

            var request = new ThrowIfNullRequest("has value");
            var sut = CreateSut(provider);
            await sut.SendAsync(request, CancellationToken.None).ConfigureAwait(false);

            await handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ShouldThrowException_WhenHandlerNotFound_ForValueRequest()
        {
            var provider = new ServiceCollection().BuildServiceProvider();
            var sut = CreateSut(provider);

            await Assert.ThrowsAsync<RequestHandlerException>(()
                => sut.SendAsync(new ReturnIfNotNullRequest("has value"), CancellationToken.None));
        }

        [Fact]
        public async Task ShouldInvokeHandler_WhenHandlerFound_ForValueRequest()
        {
            var handler = Substitute.For<IRequestHandler<ReturnIfNotNullRequest, string>>();
            var provider = new ServiceCollection()
                .AddTransient<IRequestHandler<ReturnIfNotNullRequest, string>>(_ => handler)
                .BuildServiceProvider();

            var request = new ReturnIfNotNullRequest("has value");
            var sut = CreateSut(provider);
            await sut.SendAsync(request, CancellationToken.None).ConfigureAwait(false);

            await handler.Received(1).HandleAsync(request, Arg.Any<CancellationToken>());
        }
    }
}