using JobScraper.Core.Entities;
using JobScraper.Infrastructure.Configurations;
using JobScraper.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace JobScraper.Tests.Infrastructure.Services;

public class CsvExporterTests
{
    [Fact]
    public async Task SaveAsync_WritesToFile()
    {
        // Arrange
        var mockOptions = Options.Create(new AppSettings
        {
            Csv = new CsvSettings { OutputPath = "test_jobs.csv" }
        });
        var exporter = new CsvExporter(mockOptions);

        var jobs = new List<JobPosting>
        {
            new JobPosting { Title = "Test Job", Company = "Test Company", Location = "Remote", Url = "http://test.com", PostedDate = DateTime.UtcNow, Source = "LinkedIn" }
        };

        // Act
        await exporter.SaveAsync(jobs);

        // Assert
        Assert.True(File.Exists("test_jobs.csv"));
        var content = await File.ReadAllTextAsync("test_jobs.csv");
        Assert.Contains("Test Job", content);
        File.Delete("test_jobs.csv");
    }
}
