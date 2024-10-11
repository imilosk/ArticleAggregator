using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Parsers.Interfaces;
using ArticleAggregator.Core.Repositories.Interfaces;
using ArticleAggregator.DataIngest.Services.Interfaces;
using ArticleAggregator.Settings;

namespace ArticleAggregator.DataIngest.Services.Implementations;

public class DataIngestService : IDataIngestService
{
    private readonly ILogger<DataIngestService> _logger;
    private readonly RssFeedSettings _rssFeedSettings;
    private readonly ScrapingSettings _scrapingSettings;
    private readonly IRssFeedParser _rssFeedParser;
    private readonly IXPathFeedParser _xPathFeedParser;
    private readonly IArticleRepository _articleRepository;

    public DataIngestService(
        ILogger<DataIngestService> logger,
        RssFeedSettings rssFeedSettings,
        ScrapingSettings scrapingSettings,
        IRssFeedParser rssFeedParser,
        IXPathFeedParser xPathFeedParser,
        IArticleRepository articleRepository
    )
    {
        _logger = logger;
        _rssFeedSettings = rssFeedSettings;
        _scrapingSettings = scrapingSettings;
        _rssFeedParser = rssFeedParser;
        _xPathFeedParser = xPathFeedParser;
        _articleRepository = articleRepository;
    }

    public async Task Ingest()
    {
        await ScrapeRssFeeds();
        await ScrapeHtmlFeeds();
    }

    private async Task ScrapeHtmlFeeds()
    {
        const int batchSize = 100;
        var batch = new Article[batchSize];

        foreach (var config in _scrapingSettings.XPathConfigs)
        {
            var index = 0;
            _logger.LogInformation("Scraping {baseUrl}", config.BaseUrl);

            var articles = _xPathFeedParser.ParseFromWeb(config);

            await foreach (var article in articles)
            {
                batch[index] = article;
                index++;
                if (index < 100)
                {
                    continue;
                }

                index = 0;

                if (await UpsertDataIntoDb(batch))
                {
                    _logger.LogInformation("Found existing data. Stopping scraping ...");

                    break;
                }
            }

            if (index > 0)
            {
                await UpsertDataIntoDb(batch[..index]);
            }
        }
    }

    private async Task ScrapeRssFeeds()
    {
        foreach (var config in _rssFeedSettings.FeedConfigs)
        {
            _logger.LogInformation("Scraping {baseUrl}", config.BaseUrl);

            var articles = _rssFeedParser.Parse(config.BaseUrl, config.FallbackAuthor);

            if (!await UpsertDataIntoDb(articles))
            {
                continue;
            }

            _logger.LogInformation("Found existing data. Stopping scraping ...");

            break;
        }
    }

    private async Task<bool> UpsertDataIntoDb(IEnumerable<Article> items)
    {
        var (_, updated) = await _articleRepository.UpsertMany(items);

        return updated > 0;
    }
}