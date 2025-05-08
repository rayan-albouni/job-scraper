using JobScraper.Core.DTOs;
using JobScraper.Core.Interfaces;
using JobScraper.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace JobScraper.Infrastructure.Repositories;

public class ConfigurationService : IConfigurationService
{
    private readonly AppSettings _settings;
    public ConfigurationService(IOptions<AppSettings> opts)
    {
        _settings = opts.Value;
    }

    public JobSearchConfig GetConfig(string source, string query, string location)
    {
        var cfg = new JobSearchConfig
        {
            Source = source,
            Query = query,
            Location = location,
            ResultsPerPage = _settings.Scrapers.ResultsPerPage,
            Pages = _settings.Scrapers.Pages,
            PostedWithinHours = _settings.Scrapers.PostedWithinHours
        };
        return cfg;
    }
}