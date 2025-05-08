using JobScraper.Infrastructure.Parsing;

namespace JobScraper.Tests.Infrastructure.Parsing;

public class LinkedInJobHtmlParserTests
{
    private readonly LinkedInJobHtmlParser _parser;

    public LinkedInJobHtmlParserTests()
    {
        _parser = new LinkedInJobHtmlParser();
    }

    [Fact]
    public void ParseJobDetails_ReturnsCorrectDetails_WhenHtmlIsValid()
    {
        // Arrange
        var resourcePath = Path.Combine(AppContext.BaseDirectory, "Resources", "ValidJobHtmlSample.html");
        var html = File.ReadAllText(resourcePath);

        // Act
        var result = _parser.Parse(html);

        // Assert
        Assert.Single(result);
        Assert.Equal("Job Title", result.First().Title);
        Assert.Equal("Company Name", result.First().Company);
        Assert.Equal("Location", result.First().Location);
        Assert.Equal("https://www.linkedin.com/jobs/view/12345", result.First().Url);
        Assert.Equal(new DateTime(2023, 1, 1), result.First().PostedDate);
    }

    [Fact]
    public void ParseJobDetails_ThrowsException_WhenHtmlIsInvalid()
    {
        // Arrange
        var resourcePath = Path.Combine(AppContext.BaseDirectory, "Resources", "InvalidJobHtmlSample.html");
        var invalidHtml = File.ReadAllText(resourcePath);

        var result = _parser.Parse(invalidHtml);
        Assert.Empty(result);
    }

    [Fact]
    public void ParseJobDetails_ReturnsNullFields_WhenFieldsAreMissing()
    {
        // Arrange
        var resourcePath = Path.Combine(AppContext.BaseDirectory, "Resources", "ValidJobHtmlSampleWithMissingFields.html");
        var htmlWithMissingFields = File.ReadAllText(resourcePath);

        // Act
        var result = _parser.Parse(htmlWithMissingFields);

        // Assert
        Assert.Equal("Job Title", result.First().Title);
        Assert.Empty(result.First().Company);
        Assert.Empty(result.First().Location);
    }
}
