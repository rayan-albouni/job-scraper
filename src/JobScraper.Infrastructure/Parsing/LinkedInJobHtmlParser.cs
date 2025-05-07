using System.Web;
using HtmlAgilityPack;
using JobScraper.Core.Entities;

namespace JobScraper.Infrastructure.Parsing
{
    public class LinkedInJobHtmlParser : ILinkedInJobHtmlParser
    {
        public IEnumerable<JobPosting> Parse(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // TODO: Parsed tags based on the current rendered jobs page, this should be resilient to the changes of the class names or future changes.

            var cards = doc.DocumentNode
                        .SelectNodes("//ul[contains(@class,'jobs-search__results-list')]/li/div[contains(@class,'job-search-card')]")
                    ?? Enumerable.Empty<HtmlNode>();

            foreach (var card in cards)
            {
                var urlNode = card.SelectSingleNode(".//a[contains(@class,'base-card__full-link')]");
                var href    = urlNode?.GetAttributeValue("href", "") ?? "";

                var title = card.SelectSingleNode(".//h3[contains(@class,'base-search-card__title')]")
                                ?.InnerText.Trim() ?? "";

                var company = card.SelectSingleNode(".//h4[contains(@class,'base-search-card__subtitle')]/a")
                                ?.InnerText.Trim() ?? "";

                var location = card.SelectSingleNode(".//span[contains(@class,'job-search-card__location')]")
                                ?.InnerText.Trim() ?? "";

                var datetime = card.SelectSingleNode(".//time[contains(@class,'job-search-card__listdate')]")
                                ?.GetAttributeValue("datetime", "");
                DateTime posted = DateTime.TryParse(datetime, out var dt) ? dt : DateTime.UtcNow;

                yield return new JobPosting
                {
                    Title      = HttpUtility.HtmlDecode(title),
                    Company    = HttpUtility.HtmlDecode(company),
                    Location   = HttpUtility.HtmlDecode(location),
                    Url        = href.StartsWith("http") ? href : $"https://www.linkedin.com{href}",
                    PostedDate = posted,
                    Source     = "LinkedIn"
                };
            }
        }
    }
}