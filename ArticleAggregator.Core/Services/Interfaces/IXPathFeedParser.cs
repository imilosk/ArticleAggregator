using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Settings;

namespace ArticleAggregator.Core.Services.Interfaces;

public interface IXPathFeedParser
{
    IList<Article> ParseFromWeb(XPathConfig config);
}