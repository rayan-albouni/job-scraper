using HtmlAgilityPack;
using JobScraper.Core.Entities;
using JobScraper.Core.DTOs;
using JobScraper.Core.Interfaces;
using JobScraper.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;
using JobScraper.Infrastructure.Http;
using JobScraper.Infrastructure.Parsing;

namespace JobScraper.Infrastructure.Services;
public class LinkedInJobScraper : IJobScraperService
{
    private readonly ILinkedInClient _client;
    private readonly ILinkedInJobHtmlParser _parser;
    private readonly ScraperSettings _settings;
    private readonly ILogger<LinkedInJobScraper> _log;

    public LinkedInJobScraper(ILinkedInClient client,
                              ILinkedInJobHtmlParser parser,
                              IOptions<AppSettings> opts,
                              ILogger<LinkedInJobScraper> log)
    {
        _client   = client;
        _parser   = parser;
        _settings = opts.Value.Scrapers;
        _log      = log;
    }

    public async Task<IEnumerable<JobPosting>> ScrapeAsync(JobSearchConfig config)
    {
        var all = new List<JobPosting>();
        var baseUrl = _settings.LinkedInUrl;
        var query = HttpUtility.UrlEncode(config.Query);
        var location = HttpUtility.UrlEncode(config.Location);

        for (int page = 0; page < config.Pages; page++)
        {
            var start = page * config.ResultsPerPage;
            var url = $"{baseUrl}?keywords={query}&location={location}&start={start}";
            var html = await _client.GetPageHtmlAsync(url);
            all.AddRange(_parser.Parse(html));
            await Task.Delay(_settings.PageDelayMs);
        }
        return all;
    }
}