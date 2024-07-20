using System.Globalization;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Common.WebParsingUtils;

public class HtmlLoop
{
    private readonly HtmlWeb _htmlWeb = new();

    public IEnumerable<IEnumerable<T>> Parse<T>(
        Uri baseUrl,
        string mainElementXPath,
        string nextPageXPath,
        CultureInfo cultureInfo,
        Func<XPathNavigator, T> delegateAction
    )
    {
        var currentPage = baseUrl;

        do
        {
            var htmlDocument = _htmlWeb.Load(currentPage);
            var rootNode = htmlDocument.DocumentNode ?? throw new Exception("Root element is null");

            var items = ScrapePage(rootNode, mainElementXPath, delegateAction);

            yield return items;

            currentPage = GetNextPageUrl(rootNode, baseUrl, nextPageXPath, cultureInfo);
        } while (currentPage != baseUrl);
    }

    private static Uri GetNextPageUrl(HtmlNode root, Uri baseUrl, string nextPageXPath, CultureInfo cultureInfo)
    {
        var navigator = root.CreateNavigator() ?? throw new Exception("Cannot create navigator");
        var nextPageUrl = navigator.GetValueOrDefault(nextPageXPath, string.Empty, cultureInfo);

        return UriConverter.ConvertToAbsoluteUrl(baseUrl, nextPageUrl);
    }

    private static IEnumerable<T> ScrapePage<T>(HtmlNode root, string mainElementXPath,
        Func<XPathNavigator, T> delegateAction)
    {
        var nodes = root.SelectNodes(mainElementXPath) ?? throw new Exception("No nodes found");

        foreach (var node in nodes)
        {
            var navigator = node.CreateNavigator() ?? throw new Exception("Node navigator is null");

            yield return delegateAction(navigator);
        }
    }
}