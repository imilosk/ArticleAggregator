using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.BlogGenerator.Modules;
using ArticleAggregator.BlogGenerator.ViewModels;
using Statiq.Razor;

namespace ArticleAggregator.BlogGenerator.Pipelines;

internal class ArticlesPipeline : Pipeline
{
    private const int ArticlesPerPage = 20;

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
            new RenderRazor()
                .WithLayout(new NormalizedPath("Articles.cshtml"))
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