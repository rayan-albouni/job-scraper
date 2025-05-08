using JobScraper.Core.DTOs;
using JobScraper.Core.Entities;
using JobScraper.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobScraper.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IConfigurationService _configSvc;
    private readonly IEnumerable<IJobScraperService> _scrapers;

    public JobsController(
        IConfigurationService configSvc,
        IEnumerable<IJobScraperService> scrapers)
    {
        _configSvc = configSvc;
        _scrapers = scrapers;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs(
        [FromQuery] string source,
        [FromQuery] string query,
        [FromQuery] string? location = null)
    {
        location ??= string.Empty;
        var cfg = _configSvc.GetConfig(source, query, location);

        var results = new List<JobPosting>();
        foreach (var s in _scrapers)
        {
            if (s.GetType()
                    .Name
                    .Contains(source, StringComparison.OrdinalIgnoreCase))
            {
                var jobs = await s.ScrapeAsync(cfg);
                results.AddRange(jobs);
            }
        }

        return Ok(results);
    }
}
