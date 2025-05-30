using BasicMediator.Configuration;
using BasicMediator.Exceptions;
using BasicMediator.IntegrationTests.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Xunit;

namespace BasicMediator.IntegrationTests;

public static class MediatorTests
{
    public class VoidRequest
    {
        private const string BaseUrl = "test/void";

        [Fact]
        public async Task ShouldThrowException_WhenRequestHandlerNotRegistered()
        {
            var host = await CreateHost().StartAsync();

            var client = host.GetTestClient();
            await Assert.ThrowsAsync<RequestHandlerException>(() => client.GetAsync(BaseUrl));
        }

        [Fact]
        public async Task ShouldReturnException_WhenHandlerThrowsException()
        {
            var host = await CreateHost().StartAsync();

            var client = host.GetTestClient();
            await Assert.ThrowsAsync<ArgumentNullException>(()
                => client.PostAsync(BaseUrl, new RequestModel(null).ToStringContent()));
        }

        [Fact]
        public async Task ShouldHandleRequest_WhenRequestHandledSuccessfully()
        {
            const string value = "expectation";
            var host = await CreateHost().StartAsync();

            var client = host.GetTestClient();
            var actual = await client.PostAsync(BaseUrl, new RequestModel(value).ToStringContent());
            Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
        }
    }

    public class ValueRequest
    {
        private const string BaseUrl = "test";

        [Fact]
        public async Task ShouldThrowException_WhenRequestHandlerNotRegistered()
        {
            var host = await CreateHost().StartAsync();

            var client = host.GetTestClient();
            await Assert.ThrowsAsync<RequestHandlerException>(() => client.GetAsync(BaseUrl));
        }

        [Fact]
        public async Task ShouldReturnException_WhenHandlerThrowsException()
        {
            var host = await CreateHost().StartAsync();

            var client = host.GetTestClient();
            await Assert.ThrowsAsync<ArgumentNullException>(()
                => client.PostAsync(BaseUrl, new RequestModel(null).ToStringContent()));
        }

        [Fact]
        public async Task ShouldHandleRequest_WhenRequestHandledSuccessfully()
        {
            const string value = "expectation";
            var host = await CreateHost().StartAsync();

            var client = host.GetTestClient();
            var actual = await client.PostAsync(BaseUrl, new RequestModel(value).ToStringContent());
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.Equal(value, await actual.Content.ReadAsStringAsync());
        }
    }

    private static IHostBuilder CreateHost()
        => new HostBuilder()
            .ConfigureWebHost(builder =>
                builder.UseTestServer()
                    .ConfigureServices(services => services
                        .AddBasicMediator(new BasicMediatorConfiguration
                        {
                            Assemblies = [Assembly.GetExecutingAssembly()]
                        })
                        .AddMvc())
                    .Configure(app => app
                        .UseRouting()
                        .UseEndpoints(endpoint => endpoint.MapControllers())));

    private static StringContent ToStringContent(this object obj)
    {
        var stringValue = obj as string ?? JsonSerializer.Serialize(obj);
        return new StringContent(stringValue, Encoding.UTF8, "application/json");
    }
}