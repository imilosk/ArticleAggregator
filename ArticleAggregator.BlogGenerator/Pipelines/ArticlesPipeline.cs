using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.BlogGenerator.Modules;
using ArticleAggregator.BlogGenerator.ViewModels;

using Statiq.Razor;

namespace ArticleAggregator.BlogGenerator.Pipelines;

internal class ArticlesPipeline : Pipeline
{
    private const int ArticlesPerPage = 10;

    public ArticlesPipeline(LoadArticlesModule loadArticlesModule)
    {
        InputModules =
        [
            loadArticlesModule,
        ];

        ProcessModules =
        [
            new PaginateDocuments(ArticlesPerPage),
            new ArticlePaginationModule(),
            new MergeContent(
                new ReadFiles("Index.cshtml")
            ),
            new RenderRazor()
                .WithModel(Config.FromDocument((document, _) => document.GetObject<ArticlesViewModel>())),
            new SetDestination(Config.FromDocument((document, _) =>
            {
                var model = document.GetObject<ArticlesViewModel>();

                return new NormalizedPath($"{model.CurrentPageUri}");
            })),
        ];

        OutputModules =
        [
            new WriteFiles(),
        ];
    }
}