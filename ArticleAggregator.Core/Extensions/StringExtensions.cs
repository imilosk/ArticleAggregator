using System.Net;

namespace ArticleAggregator.Core.Extensions;

public static class StringExtensions
{
    public static string HtmlDecode(this string value)
    {
        return WebUtility.HtmlDecode(value);
    }
}