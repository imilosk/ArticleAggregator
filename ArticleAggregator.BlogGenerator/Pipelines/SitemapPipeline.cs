namespace ArticleAggregator.BlogGenerator.Pipelines;

internal class SitemapPipeline : Pipeline
{
    private const string BaseUrl = "https://imilosk.github.io/relatively-general-dotnet";

    public SitemapPipeline()
    {
        Dependencies.Add(nameof(ArticlesPipeline));

        PostProcessModules =
        [
            new ExecuteConfig(Config.FromContext(ctx => ctx.Outputs.FromPipeline(nameof(ArticlesPipeline)))),
            new SetMetadata("SitemapItem",
                Config.FromDocument(doc =>
                    new SitemapItem(BaseUrl + "/" + doc.Destination))),
            new GenerateSitemap(),
        ];

        OutputModules =
        [
            new WriteFiles(),
        ];
    }
}