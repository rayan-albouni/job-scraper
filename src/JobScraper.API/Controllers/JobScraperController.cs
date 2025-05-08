using JobScraper.Core.Entities;
using JobScraper.Core.DTOs;
using JobScraper.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class JobScraperController : ControllerBase
{
    private readonly IConfigurationService _cfg;
    private readonly IEnumerable<IJobScraperService> _scrapers;

    public JobScraperController(IConfigurationService cfg, IEnumerable<IJobScraperService> scrapers)
    {
        _cfg = cfg;
        _scrapers = scrapers;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string source, [FromQuery] string query, [FromQuery] string location)
    {
        var cfg = _cfg.GetConfig(source, query, location);
        var results = new List<JobPosting>();
        foreach (var s in _scrapers)
        {
            if (s.GetType().Name.Contains(source, StringComparison.OrdinalIgnoreCase))
                results.AddRange(await s.ScrapeAsync(cfg));
        }
        return Ok(results);
    }
}

git commit --amend --author="rayan-albouni <rayanbouni@gmail.com>"