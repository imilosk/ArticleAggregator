# Tech Article Aggregator

A solution for aggregating technical articles from various sources and generating a static blog website.

## Overview

The Tech Article Aggregator is a comprehensive system designed to:
- **Collect** technical articles from RSS feeds and web scraping sources
- **Store** articles in a structured SQLite database
- **Expose** article data through REST APIs
- **Generate** static blog websites from the aggregated content

## Architecture

The solution consists of several modular components:

### Core Components

- **ArticleAggregator.Api** - REST API for accessing aggregated articles
- **ArticleAggregator.DataIngest** - Background service for collecting articles from various sources
- **ArticleAggregator.BlogGenerator** - Static site generator for creating blogs from articles
- **ArticleAggregator.DbMigrations** - Database schema migration utilities

## Features

- **Multi-Source Aggregation**: Supports both RSS feeds and XPath-based web scraping
- **RESTful API**: Paginated article retrieval with filtering capabilities
- **Static Blog Generation**: Creates modern, responsive blog websites
- **Database Management**: Automated migrations and schema management
- **Docker Support**: Containerized deployment options
- **Unit Testing**: Comprehensive test coverage

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (for containerized deployment)
- SQLite (included with .NET, no separate installation required)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd TechArticleAggregator
```

### 2. Configuration

Configure your settings in the `secrets.Development.json` files:

#### RSS Feed Configuration
Update `ArticleAggregator.Settings/secrets.Development.json` with your RSS and Scrapingfeed sources:

```json
{
  "RssFeedSettings": {
    "FeedConfigs": [
      {
        "BaseUrl": "https://example.com/rss",
        "FallbackAuthor": "Example Author"
      }
    ]
  },
  "ScrapingSettings": {
    "XPathConfigs": [
      {
        "BaseUrl": "https://example.com/page",
        "Navigation": [
          {
            "MainElementXPath": "//*[contains(@class, 'masonry-card')]"
          }
        ],
        "TitleXPath": "descendant::a/text()",
        "SummaryXPath": "descendant::p/text()",
        "LinkXPath": "descendant::a/@href",
        "AuthorXPath": "descendant::div[2]/text()",
        "PublishDateXPath": "descendant::div[3]/text()",
        "UpdateDateXPath": "",
        "IsJs": true
      }
    ]
  }
}
```

#### Database Connection
Configure your SQLite database connection string in the settings files (SQLite database file will be created automatically if it doesn't exist).

### 3. Database Setup

Run database migrations:

```bash
cd ArticleAggregator.DbMigrations
dotnet run
```

### 4. Running the Applications

#### Option A: Run Individual Components

**API Server:**
```bash
cd ArticleAggregator.Api
dotnet run
```
The API will be available at `https://localhost:5001/api/articles`

**Data Ingestion (One-time run):**
```bash
cd ArticleAggregator.DataIngest
dotnet run
```

**Blog Generator:**
```bash
cd ArticleAggregator.BlogGenerator
dotnet run
```

## API Endpoints

### GET /api/articles

Retrieve paginated articles.

**Query Parameters:**
- `page` (int, default: 1) - Page number (must be > 0)
- `pageSize` (int, default: 100) - Number of articles per page (1-1000)

**Example:**
```bash
curl "https://localhost:5001/api/articles?page=1&pageSize=10"
```

**Response:**
```json
{
  "articles": [
    {
      "id": 1,
      "title": "Example Article",
      "summary": "Article summary...",
      "author": "John Doe",
      "link": "https://example.com/article",
      "publishDate": "2024-01-01T10:00:00Z",
      "lastUpdatedTime": "2024-01-01T10:00:00Z",
      "source": "RSS"
    }
  ],
  "totalCount": 100,
  "page": 1,
  "pageSize": 10
}
```

## Project Structure

```
ArticleAggregator/
├── ArticleAggregator.Api/           # REST API service
├── ArticleAggregator.DataIngest/    # Article collection service
├── ArticleAggregator.BlogGenerator/ # Static site generator
├── ArticleAggregator.Core/          # Shared business logic
├── ArticleAggregator.Schema/        # Database schema
├── ArticleAggregator.Settings/      # Configuration models
├── ArticleAggregator.Constants/     # Shared constants
├── ArticleAggregator.DbMigrations/  # Database migrations
└── ArticleAggregator.UnitTests/     # Unit tests
```

## Data Sources

The system supports two types of article sources:

1. **RSS Feeds** - Standard RSS/Atom feed parsing
2. **XPath Scraping** - Custom web scraping using XPath selectors

Configure these sources in the secrets files.

## Deployment

### Production Deployment

The project uses a GitHub Actions workflow (`.github/workflows/dotnet.yml`) that automates the entire deployment pipeline. The workflow runs:

- **Daily at 4 AM UTC** (scheduled via cron)
- **On pushes to main branch**
- **On pull requests to main branch**

#### Deployment Process

The automated deployment follows these key steps:

1. **Build & Test**: Compiles the .NET solution in Release mode and runs all unit tests

2. **Database Management**: 
   - Downloads the existing SQLite database from the previous release (tag `v0.1`)
   - Runs database migrations to ensure schema is up-to-date

3. **Data Ingestion**:
   - Executes the data ingestion process to collect new articles from RSS feeds and web scraping sources
   - Adds fresh content to the existing database

4. **Database Publishing**:
   - Creates a new release with the updated database file (`article_aggregator.db`)
   - This updated database becomes the foundation for the next deployment cycle

5. **Static Site Generation**:
   - Builds JavaScript assets using npm and webpack
   - Generates HTML pages from the aggregated articles
   - Compiles and minifies CSS using Tailwind CSS
   - Creates a search index using Pagefind for client-side search functionality

6. **Asset Processing**:
   - Copies JavaScript, images, and favicon to the output directory
   - Prepares all static assets for deployment

7. **Deployment**:
   - Deploys the generated static blog to GitHub Pages
   - Publishes to an external repository for hosting

This workflow ensures that the blog is continuously updated with fresh content while maintaining data persistence across deployments.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add/update tests as needed
5. Submit a pull request

## Licence

This project is licenced under the MIT Licence - see the [LICENCE](LICENCE) file for details.

## Support

For issues and questions, please create an issue in the repository or contact the development team.
