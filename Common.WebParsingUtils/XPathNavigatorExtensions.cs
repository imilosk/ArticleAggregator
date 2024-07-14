using System.Globalization;
using System.Xml.XPath;
using Common.BaseTypeExtensions;

namespace Common.WebParsingUtils;

public static class XPathNavigatorExtensions
{
    public static T GetValueOrDefault<T>(
        this XPathNavigator navigator,
        string xpath,
        T defaultValue,
        CultureInfo cultureInfo
    ) where T : ISpanParsable<T>
    {
        if (string.IsNullOrEmpty(xpath))
        {
            return defaultValue;
        }

        var expressionValue = GetXpathExpressionValue(navigator, xpath);

        if (expressionValue.IsNullOrEmpty())
        {
            return defaultValue;
        }

        return T.TryParse(expressionValue, cultureInfo, out var value) ? value : defaultValue;
    }

    private static string GetXpathExpressionValue(XPathNavigator navigator, string xpath)
    {
        var expressionResult = navigator.Evaluate(xpath);

        if (expressionResult is string result)
        {
            return result;
        }

        var nodes = expressionResult as XPathNodeIterator;
        nodes?.MoveNext();
        var descendants = nodes?.Current?.SelectDescendants(XPathNodeType.All, false);

        return descendants?.Current?.Value ?? string.Empty;
    }
}