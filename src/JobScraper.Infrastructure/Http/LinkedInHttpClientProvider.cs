using JobScraper.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace JobScraper.Infrastructure.Http
{
    public class LinkedInHttpClientProvider : ILinkedInClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<LinkedInHttpClientProvider> _log;

        public LinkedInHttpClientProvider(IHttpClientFactory factory,
            IOptions<AppSettings> opts,
            ILogger<LinkedInHttpClientProvider> log,
            HttpMessageHandler? handler = null)
        {
            _http = handler == null ? factory.CreateClient("LinkedIn") : new HttpClient(handler);
            _log = log;

            var headers = opts.Value.Scrapers;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd(headers.UserAgent);
            _http.DefaultRequestHeaders.Accept.ParseAdd(headers.Accept);
            _http.DefaultRequestHeaders.AcceptLanguage.ParseAdd(headers.AcceptLanguage);
        }

        public async Task<string> GetPageHtmlAsync(string url, CancellationToken cancellationToken = default)
        {
            _log.LogDebug("Fetching LinkedIn URL: {Url}", url);
            var response = await _http.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _log.LogWarning("LinkedIn returned {StatusCode}", response.StatusCode);
                await Task.Delay(5000, cancellationToken);
                response = await _http.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}