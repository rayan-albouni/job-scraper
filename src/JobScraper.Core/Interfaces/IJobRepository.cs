using JobScraper.Core.Entities;

namespace JobScraper.Core.Interfaces
{
    public interface IJobRepository
    {
        Task SaveAsync(IEnumerable<JobPosting> jobs);
    }
}