# Job Scraper

Job Scraper is a .NET-based service for scraping job postings from platforms like LinkedIn and Indeed. It provides an API and a console application for fetching and exporting job data.

## Features

- Scrape job postings from LinkedIn and Indeed.
- Export job data to a CSV file.
- Configurable scraping settings (e.g., results per page, number of pages, etc.).
- Supports both API and console-based usage.

## Technologies Used

- **.NET 8.0**
- **ASP.NET Core** for the API.
- **HttpClient** for making HTTP requests.
- **HtmlAgilityPack** for parsing HTML.
- **Polly** for retry policies.
- **XUnit** for unit testing.

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/job-scraper.git
   cd job-scraper
   ```

2. Build the solution:
   ```bash
   dotnet build
   ```

3. Run the tests:
   ```bash
   dotnet test
   ```

## Usage

### API

1. Navigate to the API project directory:
   ```bash
   cd src/JobScraper.API
   ```

2. Run the API:
   ```bash
   dotnet run
   ```

3. Access the Swagger UI at:
   ```
   http://localhost:5238/swagger
   ```

4. Use the `/api/jobs` endpoint to fetch job postings:
   - Query parameters:
     - `source`: The job platform (e.g., `linkedin`, `indeed`).
     - `query`: The job title or keywords.
     - `location`: The job location (optional).

   Example:
   ```
   GET /api/jobs?source=linkedin&query=developer&location=remote
   ```

### Console Application

1. Navigate to the console app directory:
   ```bash
   cd src/JobScraper.ConsoleApp
   ```

2. Run the console application:
   ```bash
   dotnet run
   ```

3. Follow the prompts to enter the source, query, and location. The results will be saved to a CSV file.

## Configuration

The application settings can be customized in the `appsettings.json` file located in the `src/JobScraper.API` directory. Key settings include:

- `Scrapers.ResultsPerPage`: Number of results per page.
- `Scrapers.Pages`: Number of pages to scrape.
- `Scrapers.PostedWithinHours`: Filter jobs posted within the specified hours.
- `Csv.OutputPath`: Path to save the exported CSV file.

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a new branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add your message here"
   ```
4. Push to your branch:
   ```bash
   git push origin feature/your-feature-name
   ```
5. Open a pull request.

## License

This project is licensed under the [MIT License](LICENSE).

## Contact

For questions or support, please open an issue on the [GitHub repository](https://github.com/your-username/job-scraper).
