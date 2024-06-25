using ArticleAggregator.Core.Services.Interfaces;
using ArticleAggregator.DataIngest.Services.Interfaces;
using ArticleAggregator.Settings;

namespace ArticleAggregator.DataIngest.Services.Implementations;

public class DataIngestService : IDataIngestService
{
    private readonly RssFeedSettings _rssFeedSettings;
    private readonly ScrapingSettings _scrapingSettings;
    private readonly IRssFeedParser _rssFeedParser;
    private readonly IXPathFeedParser _xPathFeedParser;

    public DataIngestService(
        RssFeedSettings rssFeedSettings,
        ScrapingSettings scrapingSettings,
        IRssFeedParser rssFeedParser,
        IXPathFeedParser xPathFeedParser
    )
    {
        _rssFeedSettings = rssFeedSettings;
        _scrapingSettings = scrapingSettings;
        _rssFeedParser = rssFeedParser;
        _xPathFeedParser = xPathFeedParser;
    }

    public Task Ingest()
    {
        foreach (var config in _rssFeedSettings.FeedConfigs)
        {
            var items = _rssFeedParser.Parse(config.BaseUrl);
            // TODO: Write to DB
        }

        foreach (var config in _scrapingSettings.XPathConfigs)
        {
            var items = _xPathFeedParser.ParseFromWeb(config);
            // TODO: Write to DB
        }

        return Task.CompletedTask;
    }
}