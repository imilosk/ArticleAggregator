using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Parsers.Implementations;
using ArticleAggregator.Core.Parsers.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArticleAggregator.UnitTests;

public class RssFeedTests
{
    private readonly IRssFeedParser _rssFeedParser;
    private const string ExampleFilePath = "../../../../.feed-examples/feed-1.xml";
    private const string ExampleUrl = "https://lorem-rss.herokuapp.com/feed";

    public RssFeedTests()
    {
        var mock = new Mock<ILogger<RssFeedParser>>();
        var logger = mock.Object;

        _rssFeedParser = new RssFeedParser(logger);
    }

    [Fact]
    public void ParseRssFeed_LocalDiskParse_ReturnsValueTuple()
    {
        var result = _rssFeedParser.Parse(ExampleFilePath);

        Assert.IsType<List<Article>>(result);
    }

    [Fact]
    public void ParseRssFeed_UrlParse_ReturnsValueTuple()
    {
        var result = _rssFeedParser.Parse(ExampleUrl);

        Assert.IsType<List<Article>>(result);
    }
}