using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Settings;

namespace ArticleAggregator.Core.Parsers.Interfaces;

public interface IXPathFeedParser
{
    IList<Article> ParseFromWeb(XPathConfig config);
}