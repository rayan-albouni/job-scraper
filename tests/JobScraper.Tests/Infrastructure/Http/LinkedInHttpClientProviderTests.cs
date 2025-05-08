using JobScraper.Infrastructure.Configurations;
using JobScraper.Infrastructure.Http;
using JobScraper.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;

namespace JobScraper.Tests.Infrastructure.Http;
public class LinkedInHttpClientProviderTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IOptions<AppSettings>> _optionsMock;
    private readonly Mock<ILogger<LinkedInHttpClientProvider>> _loggerMock;
    private readonly HttpMessageHandlerMock _httpMessageHandlerMock;

    public LinkedInHttpClientProviderTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _optionsMock = new Mock<IOptions<AppSettings>>();
        _loggerMock = new Mock<ILogger<LinkedInHttpClientProvider>>();
        _httpMessageHandlerMock = new HttpMessageHandlerMock();
    }

    [Fact]
    public void Constructor_ShouldInitializeDependencies()
    {
        // Arrange
        var appSettings = new AppSettings
        {
            Scrapers = new ScraperSettings
            {
                UserAgent = "TestAgent",
                Accept = "text/html",
                AcceptLanguage = "en-US"
            }
        };
        _optionsMock.Setup(o => o.Value).Returns(appSettings);

        var httpClient = new HttpClient();
        _httpClientFactoryMock.Setup(f => f.CreateClient("LinkedIn")).Returns(httpClient);

        // Act
        var provider = new LinkedInHttpClientProvider(_httpClientFactoryMock.Object, _optionsMock.Object, _loggerMock.Object);

        // Assert
        Assert.NotNull(provider);
    }

    [Fact]
    public async Task GetPageHtmlAsync_ShouldReturnHtml_WhenResponseIsSuccessful()
    {
        // Arrange
        var url = "https://www.linkedin.com";
        var expectedHtml = "<html>LinkedIn</html>";
        _httpMessageHandlerMock.SetupResponse(HttpStatusCode.OK, expectedHtml);

        var httpClient = new HttpClient(_httpMessageHandlerMock);
        _httpClientFactoryMock.Setup(f => f.CreateClient("LinkedIn")).Returns(httpClient);

        var appSettings = new AppSettings
        {
            Scrapers = new ScraperSettings
            {
                UserAgent = "TestAgent",
                Accept = "text/html",
                AcceptLanguage = "en-US"
            }
        };
        _optionsMock.Setup(o => o.Value).Returns(appSettings);

        var provider = new LinkedInHttpClientProvider(_httpClientFactoryMock.Object, _optionsMock.Object, _loggerMock.Object);

        // Act
        var result = await provider.GetPageHtmlAsync(url);

        // Assert
        Assert.Equal(expectedHtml, result);
    }

    [Fact]
    public async Task GetPageHtmlAsync_ShouldRetryAndSucceed_WhenFirstResponseFails()
    {
        // Arrange
        var url = "https://www.linkedin.com";
        var expectedHtml = "<html>LinkedIn</html>";
        _httpMessageHandlerMock.SetupSequenceResponse(
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(expectedHtml) }
        );

        var httpClient = new HttpClient(_httpMessageHandlerMock);
        _httpClientFactoryMock.Setup(f => f.CreateClient("LinkedIn")).Returns(httpClient);

        var appSettings = new AppSettings
        {
            Scrapers = new ScraperSettings
            {
                UserAgent = "TestAgent",
                Accept = "text/html",
                AcceptLanguage = "en-US"
            }
        };
        _optionsMock.Setup(o => o.Value).Returns(appSettings);

        var provider = new LinkedInHttpClientProvider(_httpClientFactoryMock.Object, _optionsMock.Object, _loggerMock.Object);

        // Act
        var result = await provider.GetPageHtmlAsync(url);

        // Assert
        Assert.Equal(expectedHtml, result);
    }

    [Fact]
    public async Task GetPageHtmlAsync_ShouldThrowException_WhenAllRetriesFail()
    {
        // Arrange
        var url = "https://www.linkedin.com";
        _httpMessageHandlerMock.SetupResponse(HttpStatusCode.InternalServerError);

        var httpClient = new HttpClient(_httpMessageHandlerMock);
        _httpClientFactoryMock.Setup(f => f.CreateClient("LinkedIn")).Returns(httpClient);

        var appSettings = new AppSettings
        {
            Scrapers = new ScraperSettings
            {
                UserAgent = "TestAgent",
                Accept = "text/html",
                AcceptLanguage = "en-US"
            }
        };
        _optionsMock.Setup(o => o.Value).Returns(appSettings);

        var provider = new LinkedInHttpClientProvider(_httpClientFactoryMock.Object, _optionsMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => provider.GetPageHtmlAsync(url));
    }
}
