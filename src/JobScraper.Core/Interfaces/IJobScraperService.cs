using JobScraper.Core.DTOs;
using JobScraper.Core.Entities;

namespace JobScraper.Core.Interfaces
{
    public interface IJobScraperService
    {
        Task<IEnumerable<JobPosting>> ScrapeAsync(JobSearchConfig config);
    }
}