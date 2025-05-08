using JobScraper.Infrastructure.Configurations;
using JobScraper.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace JobScraper.Tests.Infrastructure.Repositories;

public class ConfigurationServiceTests
{
    [Fact]
    public void GetConfig_ReturnsCorrectConfig()
    {
        // Arrange
        var mockOptions = Options.Create(new AppSettings
        {
            Scrapers = new ScraperSettings { ResultsPerPage = 10, Pages = 2, PostedWithinHours = 48 }
        });
        var configService = new ConfigurationService(mockOptions);

        // Act
        var config = configService.GetConfig("LinkedIn", "developer", "remote");

        // Assert
        Assert.Equal("LinkedIn", config.Source);
        Assert.Equal("developer", config.Query);
        Assert.Equal("remote", config.Location);
        Assert.Equal(10, config.ResultsPerPage);
        Assert.Equal(2, config.Pages);
        Assert.Equal(48, config.PostedWithinHours);
    }
}
