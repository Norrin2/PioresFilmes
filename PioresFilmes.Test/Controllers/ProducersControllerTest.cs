using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using PioresFilmes.Application.Dto;
using PioresFilmes.Application.Interfaces;
using PioresFilmes.Domain;
using PioresFilmes.Test.Fixtures;
using System.Text.Json;
using Xunit;

namespace PioresFilmes.Test.Controllers
{
    [Collection("Producers")]
    public class ProducersControllerTest
    {
        private readonly TestFixture _testFixture;
        private readonly Mock<IReadCsvService> _mockCsvService;
        public ProducersControllerTest()
        {
            _mockCsvService = new Mock<IReadCsvService>();

            var expectedMin = new Producer() { Name = "Expected Min" };
            var expectedMax = new Producer() { Name = "Expected Max" };
            var producerWithNoWins = new Producer() { Name = "producerWithNoWins" };
            var producerWithOnlyOneWin = new Producer() { Name = "producerWithOnlyOneWin" };
            _mockCsvService.Setup(x => x.ReadMovies(It.IsAny<string>())).Returns(new List<Movie> {
            new Movie { Id = 1, Title = "Movie 1999", Studios = "Universal", Year = 1999, Winner = true, Producers = new List<Producer>() { expectedMin } },
            new Movie { Id = 2, Title = "Movie 1991", Studios = "Universal", Year = 1991, Winner = false, Producers = new List<Producer>() { expectedMin } },
            new Movie { Id = 3, Title = "Movie 2002", Studios = "Universal", Year = 2002, Winner = true, Producers = new List<Producer>() { expectedMin } },
            new Movie { Id = 4, Title = "Movie 2000", Studios = "Universal", Year = 2000, Winner = true, Producers = new List<Producer>() { expectedMax } },
            new Movie { Id = 5, Title = "Movie 2003", Studios = "Universal", Year = 2003, Winner = false, Producers = new List<Producer>() { expectedMax } },
            new Movie { Id = 6, Title = "Movie 2020", Studios = "Universal", Year = 2020, Winner = true, Producers = new List<Producer>() { expectedMax } },
            new Movie { Id = 7, Title = "Movie 2021", Studios = "Universal", Year = 2021, Winner = false, Producers = new List<Producer>() { producerWithNoWins } },
            new Movie { Id = 8, Title = "Movie 2022", Studios = "Universal", Year = 2022, Winner = false, Producers = new List<Producer>() { producerWithNoWins } },
            new Movie { Id = 6, Title = "Movie 2023", Studios = "Universal", Year = 2023, Winner = true, Producers = new List<Producer>() { producerWithOnlyOneWin } },
            });

            _testFixture = new TestFixture(new WebApplicationFactory<Program>(), _mockCsvService);
        }

        [Fact]
        public async Task FindProducerIntervals_ReturnsExpectedResult()
        {
            // Arrange
            var client = _testFixture.HttpClient;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Act
            var response = await client.GetAsync("/producers/intervals");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var expectedResult = JsonSerializer.Deserialize<FindProducersDto>(responseContent, options);

            // Assert
            Assert.NotNull(expectedResult);

            Assert.NotNull(expectedResult.Min);
            Assert.Equal("Expected Min", expectedResult.Min.Name);
            Assert.Equal(1999, expectedResult.Min.PreviousWin);
            Assert.Equal(2002, expectedResult.Min.FollowingWin);
            Assert.Equal(3, expectedResult.Min.Interval);

            Assert.NotNull(expectedResult.Max);
            Assert.Equal("Expected Max", expectedResult.Max.Name);
            Assert.Equal(2000, expectedResult.Max.PreviousWin);
            Assert.Equal(2020, expectedResult.Max.FollowingWin);
            Assert.Equal(20, expectedResult.Max.Interval);
        }
    }
}
