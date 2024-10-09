using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Parsers.Implementations;
using ArticleAggregator.Core.Parsers.Interfaces;
using ArticleAggregator.Settings;
using IMilosk.WebParsingUtils;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArticleAggregator.UnitTests;

public class XPathTests
{
    private readonly IXPathFeedParser _xPathFeedParser;

    public XPathTests()
    {
        var mock = new Mock<ILogger<XPathFeedParser>>();
        var logger = mock.Object;

        _xPathFeedParser = new XPathFeedParser(logger);
    }

    [Fact]
    public async Task ParsePage_XPathParse_ReturnsValueTuple()
    {
        var config = new XPathConfig
        {
            BaseUrl = new Uri("https://webdevbev.co.uk/blog.html"),
            Navigation =
            [
                new Navigation
                {
                    MainElementXPath = "//*[contains(@class, 'blog__post-preview')]",
                }
            ],

            TitleXPath = "descendant::h2//a/text()",
            SummaryXPath = "descendant::p[3]/text()",
            LinkXPath = "descendant::h2//a/@href",
            AuthorXPath = string.Empty,
            PublishDateXPath = string.Empty,
            UpdateDateXPath = string.Empty,
        };

        var result = _xPathFeedParser.ParseFromWeb(config);

        await foreach (var article in result)
        {
            Assert.IsType<Article>(article);
        }
    }
}