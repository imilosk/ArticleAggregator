using System.ServiceModel.Syndication;
using System.Xml;
using ArticleAggregator.Constants;
using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Extensions;
using ArticleAggregator.Core.Parsers.Interfaces;
using Common.BaseTypeExtensions;
using Common.WebParsingUtils;
using Microsoft.Extensions.Logging;

namespace ArticleAggregator.Core.Parsers.Implementations;

public class RssFeedParser : IRssFeedParser
{
    private readonly ILogger<RssFeedParser> _logger;

    public RssFeedParser(ILogger<RssFeedParser> logger)
    {
        _logger = logger;
    }

    public List<Article> Parse(string url, string fallbackAuthor)
    {
        using var xmlReader = XmlReader.Create(url);

        return Parse(xmlReader, fallbackAuthor);
    }

    public List<Article> Parse(XmlReader xmlReader, string fallbackAuthor)
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
                Title = item.Title.Text.Trim(),
                Summary = StripHtml.StripHtmlTagsRegex().Replace(
                    item.Summary.Text,
                    string.Empty
                ).HtmlDecode().Trim(),
                Author = item.Authors.Count > 0 ? item.Authors.First().Name : fallbackAuthor,
                Link = item.Links.First().Uri,
                PublishDate = item.PublishDate.DateTime,
                LastUpdatedTime = item.LastUpdatedTime.DateTime,
                Source = ArticleSource.Rss,
            });
        }

        _logger.LogInformation("Parsed RSS Feed");

        return items;
    }
}