namespace JobScraper.Core.DTOs
{
    public class JobSearchConfig
    {
        public string Source { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int ResultsPerPage { get; set; } = 20;
        public int Pages { get; set; } = 1;
    }
}