using Common.WebParsingUtils;

namespace ArticleAggregator.UnitTests;

public class UriConverterTests
{
    [Theory]
    [InlineData("https://milos.com/", "/blog/post/1", "https://milos.com/blog/post/1")]
    [InlineData("https://milos.com/", "", "https://milos.com/")]
    public void CompleteUri_LocalDiskParse_ReturnsValueTuple(string baseUri, string path, string expected)
    {
        var result = UriConverter.ConvertToAbsoluteUrl(new Uri(baseUri), path);

        Assert.Equal(new Uri(expected), result);
    }
}