using JobScraper.Core.Entities;

namespace JobScraper.Infrastructure.Parsing
{
    public interface ILinkedInJobHtmlParser
    {
        IEnumerable<JobPosting> Parse(string html);
    }
}