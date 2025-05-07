namespace JobScraper.Core.Entities
{
    public class JobPosting
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}