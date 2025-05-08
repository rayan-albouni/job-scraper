using JobScraper.Core.Interfaces;
using JobScraper.Infrastructure.Repositories;
using JobScraper.Infrastructure.Configurations;
using JobScraper.Infrastructure.Http;
using JobScraper.Infrastructure.Parsing;
using JobScraper.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobScraper.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AppSettings>(config);
        services.AddHttpClient();
        services.AddSingleton<IJobRepository, CsvExporter>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddSingleton<ILinkedInClient, LinkedInHttpClientProvider>();
        services.AddSingleton<ILinkedInJobHtmlParser, LinkedInJobHtmlParser>();
        services.AddTransient<IJobScraperService, LinkedInJobScraper>();
        services.AddTransient<IJobScraperService, IndeedJobScraper>();
        return services;
    }
}
