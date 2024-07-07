using System.ServiceModel.Syndication;
using System.Xml;
using ArticleAggregator.Constants;
using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Parsers.Interfaces;
using Common.BaseTypeExtensions;
using Microsoft.Extensions.Logging;

namespace ArticleAggregator.Core.Parsers.Implementations;

public class RssFeedParser : IRssFeedParser
{
    private readonly ILogger<RssFeedParser> _logger;

    public RssFeedParser(ILogger<RssFeedParser> logger)
    {
        _logger = logger;
    }

    public List<Article> Parse(string url)
    {
        using var xmlReader = XmlReader.Create(url);

        return Parse(xmlReader);
    }

    public List<Article> Parse(XmlReader xmlReader)
    {
        var feed = SyndicationFeed.Load(xmlReader);

        var items = new List<Article>();
        foreach (var item in feed.Items)
        {
            if (item.Title.Text.IsNullOrEmpty())
            {
                _logger.LogWarning("Skipped one item because of empty title");

                continue;
            }

            items.Add(new Article
            {
                Title = item.Title.Text,
                Summary = item.Summary.Text,
                Author = item.Authors.Count > 0 ? item.Authors.First().Name : string.Empty, // TODO: Add author fallback
                LinkUri = item.Links.First().Uri,
                PublishDate = item.PublishDate.DateTime,
                LastUpdatedTime = item.LastUpdatedTime.DateTime,
                Source = ArticleSource.Rss,
            });
        }

        _logger.LogInformation("Parsed RSS Feed");

        return items;
    }
}