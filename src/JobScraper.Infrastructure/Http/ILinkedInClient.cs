namespace JobScraper.Infrastructure.Http
{
    public interface ILinkedInClient
    {
        Task<string> GetPageHtmlAsync(string url, CancellationToken cancellationToken = default);
    }
}