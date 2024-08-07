using ArticleAggregator.BlogGenerator.CustomExtensions;
using ArticleAggregator.BlogGenerator.Modules;
using Statiq.Razor;

namespace ArticleAggregator.BlogGenerator.Pipelines;

public class ArticlesPipeline : Pipeline
{
    public ArticlesPipeline()
    {
        InputModules =
        [
            new LoadArticlesModule()
        ];

        ProcessModules =
        [
            new MergeContent(new ReadFiles(patterns: "Article.cshtml")),
            new RenderRazor().WithModel(Config.FromDocument((document, _) => document.ToArticleViewModel())),
            new SetDestination(Config.FromDocument((_, _) => new NormalizedPath("index.html")))
        ];

        OutputModules =
        [
            new WriteFiles()
        ];
    }
}