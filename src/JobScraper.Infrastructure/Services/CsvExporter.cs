using JobScraper.Core.Entities;
using JobScraper.Core.Interfaces;
using JobScraper.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Text;

namespace JobScraper.Infrastructure.Services;

public class CsvExporter : IJobRepository
{
    private readonly CsvSettings _csv;
    public CsvExporter(IOptions<AppSettings> opts) => _csv = opts.Value.Csv;

    public async Task SaveAsync(IEnumerable<JobPosting> jobs)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Title,Company,Location,Url,PostedDate,Source");
        foreach (var j in jobs)
            sb.AppendLine($"\"{j.Title}\",\"{j.Company}\",\"{j.Location}\",{j.Url},{j.PostedDate:O},{j.Source}");
        await File.WriteAllTextAsync(_csv.OutputPath, sb.ToString());
    }
}