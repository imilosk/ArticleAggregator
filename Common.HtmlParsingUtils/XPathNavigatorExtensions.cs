using System.Globalization;
using System.Xml.XPath;
using Common.BaseTypeExtensions;

namespace Common.HtmlParsingUtils;

public static class XPathNavigatorExtensions
{
    public static T GetValueOrDefault<T>(this XPathNavigator navigator, string xpath, T defaultValue,
        CultureInfo cultureInfo)
        where T : ISpanParsable<T>
    {
        if (xpath.IsNullOrEmpty())
        {
            return defaultValue;
        }

        var selectedNode = navigator.SelectSingleNode(xpath);

        if (selectedNode is null)
        {
            return defaultValue;
        }

        return T.TryParse(selectedNode.Value, cultureInfo, out var value) ? value : defaultValue;
    }
}