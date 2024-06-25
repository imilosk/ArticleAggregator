using System.Globalization;
using System.Xml.XPath;
using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Services.Interfaces;
using ArticleAggregator.Settings;
using Common.HtmlParsingTools;
using Microsoft.Extensions.Logging;

namespace ArticleAggregator.Core.Services.Implementations;

public class XPathFeedParser : IXPathFeedParser
{
    private readonly ILogger<XPathFeedParser> _logger;
    private readonly HtmlLoop<Article> _htmlLoop;
    private static readonly CultureInfo DefaultCultureInfo = CultureInfo.InvariantCulture;

    public XPathFeedParser(ILogger<XPathFeedParser> logger)
    {
        _logger = logger;
        _htmlLoop = new HtmlLoop<Article>();
    }

    public IList<Article> ParseFromWeb(XPathConfig config)
    {
        return _htmlLoop.Parse(
            config.BaseUrl,
            config.ArticleXPath,
            config.NextPageXPath,
            DefaultCultureInfo,
            DelegateAction
        );

        Article DelegateAction(XPathNavigator navigator) => ParseArticle(navigator, config);
    }

    private Article ParseArticle(XPathNavigator navigator, XPathConfig config)
    {
        var article = new Article
        {
            Title = navigator.GetValueOrDefault(config.TitleXPath, string.Empty, DefaultCultureInfo),
            Summary = navigator.GetValueOrDefault(config.SummaryXPath, string.Empty, DefaultCultureInfo),
            Author = navigator.GetValueOrDefault(config.AuthorXPath, string.Empty, DefaultCultureInfo),
            Link = new Uri(navigator.GetValueOrDefault(config.LinkXPath, string.Empty, DefaultCultureInfo)),
            PublishDate =
                navigator.GetValueOrDefault(config.PublishDateXPath, DateTime.MinValue, DefaultCultureInfo),
            LastUpdatedTime =
                navigator.GetValueOrDefault(config.UpdateDateXPath, DateTime.MinValue, DefaultCultureInfo),
        };

        _logger.LogInformation("Parsed element with title: {title}", article.Title);

        return article;
    }
}