using System.Globalization;
using System.Xml.XPath;
using ArticleAggregator.Constants;
using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Extensions;
using ArticleAggregator.Core.Parsers.Interfaces;
using ArticleAggregator.Settings;
using Common.WebParsingUtils;
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

    public IEnumerable<IEnumerable<Article>> ParseFromWeb(XPathConfig config)
    {
        var pages = _htmlLoop.Parse(
            config.BaseUrl,
            config.ArticleXPath,
            config.NextPageXPath,
            DefaultCultureInfo,
            config.IsJs,
            navigator => ParseArticle(navigator, config)
        );

        return pages;
    }

    private Article ParseArticle(XPathNavigator navigator, XPathConfig config)
    {
        var article = new Article
        {
            Title =
                navigator
                    .GetValueOrDefault(config.TitleXPath, string.Empty, DefaultCultureInfo)
                    .HtmlDecode().Trim(),
            Summary = StripHtml.StripHtmlTagsRegex().Replace(
                    navigator.GetValueOrDefault(config.SummaryXPath, string.Empty, DefaultCultureInfo),
                    string.Empty)
                .HtmlDecode().Trim(),
            Author = navigator.GetValueOrDefault(config.AuthorXPath, string.Empty, DefaultCultureInfo),
            Link = UriConverter.ConvertToAbsoluteUrl(
                config.BaseUrl,
                navigator.GetValueOrDefault(config.LinkXPath, string.Empty, DefaultCultureInfo)
            ),
            PublishDate =
                navigator.GetValueOrDefault(config.PublishDateXPath, DateTime.MinValue, DefaultCultureInfo),
            LastUpdatedTime =
                navigator.GetValueOrDefault(config.UpdateDateXPath, DateTime.MinValue, DefaultCultureInfo),
            Source = ArticleSource.XPath,
        };

        _logger.LogInformation("Parsed element with title: {title}", article.Title);

        return article;
    }
}