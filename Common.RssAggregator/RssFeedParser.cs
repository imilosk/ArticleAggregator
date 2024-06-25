using System.ServiceModel.Syndication;
using System.Xml;
using ArticleAggregator.DataModels;

namespace Common.RssAggregator;

public class RssFeedParser
{
    public static List<Article> ParseRssFeed(string url)
    {
        using var xmlReader = XmlReader.Create(url);

        return ParseRssFeed(xmlReader);
    }

    public static List<Article> ParseRssFeed(XmlReader xmlReader)
    {
        var feed = SyndicationFeed.Load(xmlReader);

        var items = new List<Article>();
        foreach (var item in feed.Items)
        {
            items.Add(new Article
            {
                Title = item.Title.Text,
                Summary = item.Summary.Text,
                Author = item.Authors.Count > 0 ? item.Authors.First().Name : string.Empty, // TODO: Add author fallback
                PublishDate = item.PublishDate.DateTime,
                LastUpdatedTime = item.LastUpdatedTime.DateTime,
            });
        }

        return items;
    }
}