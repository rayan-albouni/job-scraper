namespace JobScraper.Infrastructure.Configurations
{
    public class AppSettings
    {
        public ScraperSettings Scrapers { get; set; } = new();
        public CsvSettings Csv { get; set; } = new();
    }

    public class ScraperSettings 
    { 
        public string LinkedInUrl { get; set; } = string.Empty; 
        public string IndeedUrl { get; set; } = string.Empty; 
        public int ResultsPerPage { get; set; } = 20;
        public int Pages { get; set; } = 1;
        public int PostedWithinHours { get; set; } = 24; 
        public int PageDelayMs { get; set; } = 100;
        public string UserAgent { get; set; } = string.Empty; 
        public string Accept { get; set; } = string.Empty; 
        public string AcceptLanguage { get; set; } = string.Empty; 
    }
    public class CsvSettings { public string OutputPath { get; set; } = "jobs.csv"; }
}