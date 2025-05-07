using JobScraper.Core.Entities;
using JobScraper.Core.Interfaces;
using JobScraper.Infrastructure.Configurations;
using JobScraper.Infrastructure.Http;
using JobScraper.Infrastructure.Parsing;
using JobScraper.Infrastructure.Repositories;
using JobScraper.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json", optional: false))
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<AppSettings>(ctx.Configuration);
        services.AddHttpClient<LinkedInJobScraper>().AddTransientHttpErrorPolicy(p => p.RetryAsync(3));
        services.AddHttpClient<IndeedJobScraper>().AddTransientHttpErrorPolicy(p => p.RetryAsync(3));
        services.AddSingleton<IJobRepository, CsvExporter>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<ILinkedInClient, LinkedInHttpClientProvider>();
        services.AddSingleton<ILinkedInJobHtmlParser, LinkedInJobHtmlParser>();
        services.AddTransient<IJobScraperService, LinkedInJobScraper>();
        services.AddTransient<IJobScraperService, IndeedJobScraper>();

    })
    .Build();

var configSvc = host.Services.GetRequiredService<IConfigurationService>();
var repo = host.Services.GetRequiredService<IJobRepository>();
var scrapers = host.Services.GetServices<IJobScraperService>();

Console.WriteLine("Enter source (linkedin/indeaed):");
var source = Console.ReadLine()!;
Console.WriteLine("Enter query:");
var query = Console.ReadLine()!;
Console.WriteLine("Enter Location:");
var location = Console.ReadLine()!;
var cfg = configSvc.GetConfig(source, query, location);

var allJobs = new List<JobPosting>();
foreach (var s in scrapers)
{
    if (s.GetType().Name.Contains(source, StringComparison.OrdinalIgnoreCase))
        allJobs.AddRange(await s.ScrapeAsync(cfg));
}

await repo.SaveAsync(allJobs);
Console.WriteLine($"Saved {allJobs.Count} jobs to CSV.");