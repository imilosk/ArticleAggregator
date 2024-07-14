using System.Globalization;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Common.WebParsingUtils;

public class HtmlLoop
{
    private readonly HtmlWeb _htmlWeb = new();

    public void Parse(
        Uri baseUrl,
        string mainElementXPath,
        string nextPageXPath,
        CultureInfo cultureInfo,
        Action<XPathNavigator> delegateAction
    )
    {
        var currentPage = baseUrl;

        do
        {
            var htmlDocument = _htmlWeb.Load(currentPage);
            var rootNode = htmlDocument.DocumentNode ?? throw new Exception("Root element is null");

            var itemsAddedCount = ScrapePage(rootNode, mainElementXPath, delegateAction);

            if (itemsAddedCount == 0)
            {
                break;
            }

            currentPage = GetNextPageUrl(rootNode, baseUrl, nextPageXPath, cultureInfo);
        } while (currentPage != baseUrl);
    }

    private static Uri GetNextPageUrl(HtmlNode root, Uri baseUrl, string nextPageXPath, CultureInfo cultureInfo)
    {
        var navigator = root.CreateNavigator() ?? throw new Exception("Cannot create navigator");
        var nextPageUrl = navigator.GetValueOrDefault(nextPageXPath, string.Empty, cultureInfo);

        return UriConverter.ConvertToAbsoluteUrl(baseUrl, nextPageUrl);
    }

    private static int ScrapePage(HtmlNode root, string mainElementXPath, Action<XPathNavigator> delegateAction)
    {
        var nodes = root.SelectNodes(mainElementXPath) ?? throw new Exception("No nodes found");

        foreach (var node in nodes)
        {
            var navigator = node.CreateNavigator() ?? throw new Exception("Node navigator is null");
            delegateAction(navigator);
        }

        return nodes.Count;
    }
}