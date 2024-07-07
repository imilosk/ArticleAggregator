using System.Globalization;
using System.Xml.XPath;
using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Parsers.Interfaces;
using ArticleAggregator.Settings;
using Common.HtmlParsingTools;
using Microsoft.Extensions.Logging;

namespace ArticleAggregator.Core.Parsers.Implementations;

public class XPathFeedParser : IXPathFeedParser
{
    private readonly ILogger<XPathFeedParser> _logger;
    private readonly HtmlLoop _htmlLoop;
    private static readonly CultureInfo DefaultCultureInfo = CultureInfo.InvariantCulture;

    public XPathFeedParser(ILogger<XPathFeedParser> logger)
    {
        _logger = logger;
        _htmlLoop = new HtmlLoop();
    }

    public IList<Article> ParseFromWeb(XPathConfig config)
    {
        var items = new List<Article>();

        _htmlLoop.Parse(
            config.BaseUrl,
            config.ArticleXPath,
            config.NextPageXPath,
            DefaultCultureInfo,
            DelegateAction
        );

        return items;

        void DelegateAction(XPathNavigator navigator) => ParseArticle(navigator, config, items);
    }

    private void ParseArticle(XPathNavigator navigator, XPathConfig config, List<Article> items)
    {
        var article = new Article
        {
            Title = navigator.GetValueOrDefault(config.TitleXPath, string.Empty, DefaultCultureInfo),
            Summary = navigator.GetValueOrDefault(config.SummaryXPath, string.Empty, DefaultCultureInfo),
            Author = navigator.GetValueOrDefault(config.AuthorXPath, string.Empty, DefaultCultureInfo),
            LinkUri = new Uri(navigator.GetValueOrDefault(config.LinkXPath, string.Empty, DefaultCultureInfo)),
            PublishDate =
                navigator.GetValueOrDefault(config.PublishDateXPath, DateTime.MinValue, DefaultCultureInfo),
            LastUpdatedTime =
                navigator.GetValueOrDefault(config.UpdateDateXPath, DateTime.MinValue, DefaultCultureInfo),
        };

        items.Add(article);

        _logger.LogInformation("Parsed element with title: {title}", article.Title);
    }
}