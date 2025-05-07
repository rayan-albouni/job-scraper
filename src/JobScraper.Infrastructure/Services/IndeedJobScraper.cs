using JobScraper.Core.Entities;
using JobScraper.Core.DTOs;
using JobScraper.Core.Interfaces;
using JobScraper.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JobScraper.Infrastructure.Services;
public class IndeedJobScraper : IJobScraperService
{
    private readonly HttpClient _http;
    private readonly ScraperSettings _settings;
    private readonly ILogger<IndeedJobScraper> _log;

    public IndeedJobScraper(HttpClient http, IOptions<AppSettings> opts, ILogger<IndeedJobScraper> log)
    {
        _http = http;
        _settings = opts.Value.Scrapers;
        _log = log;
    }
    public async Task<IEnumerable<JobPosting>> ScrapeAsync(JobSearchConfig config)
    {
        // TODO: implement real scraping logic with HTML parsing
        _log.LogInformation("Scraping Indeed for {Query}", config.Query);
        await Task.Delay(500);
        return new List<JobPosting>();
    }
}