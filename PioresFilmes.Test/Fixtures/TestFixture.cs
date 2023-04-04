using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PioresFilmes.Application.Interfaces;
using Xunit;

namespace PioresFilmes.Test.Fixtures
{
    public class TestFixture: IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        public HttpClient HttpClient { get; private set; }

        private readonly Mock<IReadCsvService> _mockCsvService;
        private readonly WebApplicationFactory<Program> _factory;
        public TestFixture(WebApplicationFactory<Program> factory,
                           Mock<IReadCsvService> mockCsvService)
        {
            _factory = factory;
            _mockCsvService = mockCsvService;

            var builder = WebApplication.CreateBuilder();
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.Remove(services.First(d => d.ServiceType == typeof(IReadCsvService)));
                    services.AddSingleton(_mockCsvService.Object);
                });
            }).CreateClient();

            HttpClient = client;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
