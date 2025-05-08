using JobScraper.Core.DTOs;
using JobScraper.Infrastructure.Configurations;
using JobScraper.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace JobScraper.Tests.Infrastructure.Services;

public class IndeedJobScraperTests
{
    [Fact]
    public async Task ScrapeAsync_LogsInformation()
    {
        // Arrange
        var mockHttpClient = new Mock<HttpClient>();
        var mockLogger = new Mock<ILogger<IndeedJobScraper>>();
        var mockOptions = Options.Create(new AppSettings
        {
            Scrapers = new ScraperSettings { PageDelayMs = 0 }
        });

        var scraper = new IndeedJobScraper(mockHttpClient.Object, mockOptions, mockLogger.Object);

        // Act
        var result = await scraper.ScrapeAsync(new JobSearchConfig { Query = "test", Location = "remote" });

        // Assert
        Assert.Empty(result);
        mockLogger.Verify(
            static l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }
}