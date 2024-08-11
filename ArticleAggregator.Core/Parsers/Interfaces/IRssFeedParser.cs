using System.Xml;
using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.Core.Parsers.Interfaces;

public interface IRssFeedParser
{
    List<Article> Parse(string url, string fallbackAuthor);
    List<Article> Parse(XmlReader xmlReader, string fallbackAuthor);
}