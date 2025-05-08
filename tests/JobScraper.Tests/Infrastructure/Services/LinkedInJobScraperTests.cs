using JobScraper.Core.DTOs;
using JobScraper.Core.Entities;
using JobScraper.Infrastructure.Configurations;
using JobScraper.Infrastructure.Http;
using JobScraper.Infrastructure.Parsing;
using JobScraper.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace JobScraper.Tests.Infrastructure.Services;

public class LinkedInJobScraperTests
{
    [Fact]
    public async Task ScrapeAsync_ReturnsJobPostings()
    {
        // Arrange
        var mockClient = new Mock<ILinkedInClient>();
        var mockParser = new Mock<ILinkedInJobHtmlParser>();
        var mockLogger = new Mock<ILogger<LinkedInJobScraper>>();
        var mockOptions = Options.Create(new AppSettings
        {
            Scrapers = new ScraperSettings
            {
                LinkedInUrl = "https://www.linkedin.com/jobs/search",
                PageDelayMs = 0
            }
        });

        mockClient.Setup(c => c.GetPageHtmlAsync(It.IsAny<string>(), default))
                  .ReturnsAsync("<html></html>");
        mockParser.Setup(p => p.Parse(It.IsAny<string>()))
                  .Returns(new List<JobPosting> { new JobPosting { Title = "Test Job" } });

        var scraper = new LinkedInJobScraper(mockClient.Object, mockParser.Object, mockOptions, mockLogger.Object);

        // Act
        var result = await scraper.ScrapeAsync(new JobSearchConfig { Query = "test", Location = "remote", Pages = 1 });

        // Assert
        Assert.Single(result);
        Assert.Equal("Test Job", result.First().Title);
    }
}