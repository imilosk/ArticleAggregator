using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Settings;

namespace ArticleAggregator.Core.Parsers.Interfaces;

public interface IXPathFeedParser
{
    IEnumerable<IEnumerable<Article>> ParseFromWeb(XPathConfig config);
}