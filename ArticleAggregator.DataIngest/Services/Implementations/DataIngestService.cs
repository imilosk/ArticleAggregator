using ArticleAggregator.Core.Parsers.Interfaces;
using ArticleAggregator.Core.Repositories.Interfaces;
using ArticleAggregator.DataIngest.Services.Interfaces;
using ArticleAggregator.Settings;

namespace ArticleAggregator.DataIngest.Services.Implementations;

public class DataIngestService : IDataIngestService
{
    private readonly RssFeedSettings _rssFeedSettings;
    private readonly ScrapingSettings _scrapingSettings;
    private readonly IRssFeedParser _rssFeedParser;
    private readonly IXPathFeedParser _xPathFeedParser;
    private readonly IArticleRepository _articleRepository;

    public DataIngestService(
        RssFeedSettings rssFeedSettings,
        ScrapingSettings scrapingSettings,
        IRssFeedParser rssFeedParser,
        IXPathFeedParser xPathFeedParser,
        IArticleRepository articleRepository
    )
    {
        _rssFeedSettings = rssFeedSettings;
        _scrapingSettings = scrapingSettings;
        _rssFeedParser = rssFeedParser;
        _xPathFeedParser = xPathFeedParser;
        _articleRepository = articleRepository;
    }

    public Task Ingest()
    {
        foreach (var config in _rssFeedSettings.FeedConfigs)
        {
            var items = _rssFeedParser.Parse(config.BaseUrl);

            _articleRepository.CreateMany(items);
        }

        foreach (var config in _scrapingSettings.XPathConfigs)
        {
            var items = _xPathFeedParser.ParseFromWeb(config);

            _articleRepository.CreateMany(items);
        }

        return Task.CompletedTask;
    }
}