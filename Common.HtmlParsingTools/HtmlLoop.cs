using System.Globalization;
using System.Xml.XPath;
using Common.BaseTypeExtensions;
using HtmlAgilityPack;

namespace Common.HtmlParsingTools;

public class HtmlLoop<T>
{
    private readonly HtmlWeb _htmlWeb = new();

    public IList<T> Parse(
        string url,
        string mainElementXPath,
        string nextPageXPath,
        CultureInfo cultureInfo,
        Func<XPathNavigator, T> delegateAction
    )
    {
        var items = new List<T>();
        var currentPage = url;

        while (!currentPage.IsNullOrEmpty())
        {
            var htmlDocument = _htmlWeb.Load(currentPage);
            var rootNode = htmlDocument.DocumentNode ?? throw new Exception("Root element is null");

            var itemsAddedCount = ScrapePage(rootNode, items, mainElementXPath, delegateAction);

            if (itemsAddedCount == 0)
            {
                break;
            }

            currentPage = GetNextPageUrl(rootNode, nextPageXPath, cultureInfo);
        }

        return items;
    }

    private static string? GetNextPageUrl(HtmlNode root, string nextPageXPath, CultureInfo cultureInfo)
    {
        var navigator = root.CreateNavigator();

        return navigator?.GetValueOrDefault(nextPageXPath, string.Empty, cultureInfo);
    }

    private static int ScrapePage(
        HtmlNode root,
        IList<T> items,
        string mainElementXPath,
        Func<XPathNavigator, T> delegateAction
    )
    {
        var nodes = root.SelectNodes(mainElementXPath) ?? throw new Exception("No nodes found");

        foreach (var node in nodes)
        {
            var navigator = node.CreateNavigator() ?? throw new Exception("Node navigator is null");
            var item = delegateAction(navigator);
            items.Add(item);
        }

        return nodes.Count;
    }
}