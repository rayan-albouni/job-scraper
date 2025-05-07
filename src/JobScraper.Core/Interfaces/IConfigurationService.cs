using JobScraper.Core.DTOs;

namespace JobScraper.Core.Interfaces
{
    public interface IConfigurationService
    {
        JobSearchConfig GetConfig(string source, string query, string location);
    }
}
