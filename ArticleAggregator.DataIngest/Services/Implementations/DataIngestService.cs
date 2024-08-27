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
        foreach (var config in _rssFeedSettings.FeedConfigs)
        {
            _logger.LogInformation($"Scraping {config.BaseUrl}");
            
            var articles = _rssFeedParser.Parse(config.BaseUrl, config.FallbackAuthor);

            if (await InsertDataIntoDb(articles))
            {
                break;
            }
        }

        foreach (var config in _scrapingSettings.XPathConfigs)
        {
            _logger.LogInformation($"Scraping {config.BaseUrl}");
            
            var pages = _xPathFeedParser.ParseFromWeb(config);

            await foreach (var articles in pages)
            {
                if (await InsertDataIntoDb(articles))
                {
                    break;
                }
            }
        }
    }

    private async Task<bool> InsertDataIntoDb(IEnumerable<Article> items)
    {
        var (_, updated) = await _articleRepository.UpsertMany(items);

        if (updated <= 0)
        {
            return false;
        }

        _logger.LogInformation("Found existing data. Stopping scraping ...");
        return true;
    }
}