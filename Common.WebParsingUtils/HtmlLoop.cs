using System.Globalization;
using System.Xml.XPath;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace Common.WebParsingUtils;

public class HtmlLoop
{
    private readonly HtmlWeb _htmlWeb = new();
    private IBrowser? _browser;

    public async IAsyncEnumerable<IEnumerable<T>> Parse<T>(
        Uri baseUrl,
        string mainElementXPath,
        string nextPageXPath,
        CultureInfo cultureInfo,
        bool isJs,
        Func<XPathNavigator, T> delegateAction
    )
    {
        var browser = await InitiateBrowser();
        var currentPage = baseUrl;

        do
        {
            var htmlDocument = await LoadHtmlDocument(browser, currentPage, isJs);
            var rootNode = htmlDocument.DocumentNode ?? throw new Exception("Root element is null");
            var items = ScrapePage(rootNode, mainElementXPath, delegateAction);

            yield return items;

            currentPage = GetNextPageUrl(rootNode, baseUrl, nextPageXPath, cultureInfo);
        } while (currentPage != baseUrl);
    }

    private async Task<IBrowser> InitiateBrowser()
    {
        await new BrowserFetcher().DownloadAsync();

        return _browser ??= await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
    }

    private async Task<HtmlDocument> LoadHtmlDocument(IBrowser browser, Uri url, bool isJs)
    {
        if (!isJs)
        {
            return _htmlWeb.Load(url);
        }

        var page = await browser.NewPageAsync();
        await page.GoToAsync(url.ToString(), 120000);

        // TODO: Figure out something better
        await Task.Delay(10000);

        var htmlContent = await page.GetContentAsync();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        return htmlDocument;
    }

    private static Uri GetNextPageUrl(HtmlNode root, Uri baseUrl, string nextPageXPath, CultureInfo cultureInfo)
    {
        var navigator = root.CreateNavigator() ?? throw new Exception("Cannot create navigator");
        var nextPageUrl = navigator.GetValueOrDefault(nextPageXPath, string.Empty, cultureInfo);

        return UriConverter.ConvertToAbsoluteUrl(baseUrl, nextPageUrl);
    }

    private static IEnumerable<T> ScrapePage<T>(
        HtmlNode root,
        string mainElementXPath,
        Func<XPathNavigator, T> delegateAction
    )
    {
        var nodes = root.SelectNodes(mainElementXPath);

        if (nodes is null)
        {
            yield break;
        }

        foreach (var node in nodes)
        {
            var navigator = node.CreateNavigator() ?? throw new Exception("Node navigator is null");

            yield return delegateAction(navigator);
        }
    }
}