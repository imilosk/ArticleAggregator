using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Settings;

namespace ArticleAggregator.Core.Parsers.Interfaces;

public interface IXPathFeedParser
{
    IAsyncEnumerable<IEnumerable<Article>> ParseFromWeb(XPathConfig config);
}