using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.BlogGenerator.Modules;
using ArticleAggregator.BlogGenerator.ViewModels;
using ArticleAggregator.Core.DataModels;
using Statiq.Razor;

namespace ArticleAggregator.BlogGenerator.Pipelines;

public class HomePipeline : Pipeline
{
    public HomePipeline()
    {
        InputModules =
        [
            new LoadArticlesModule()
        ];

        ProcessModules =
        [
            new ExecuteConfig(
                Config.FromContext(executionContext =>
                {
                    var articles = executionContext.Inputs.GetObjects<Article>(executionContext);

                    var homeViewModel = new HomeViewModel
                    {
                        Articles = articles,
                    };

                    return homeViewModel.ToDocument(executionContext).Yield();
                })
            ),
            new RenderRazor()
                .WithLayout(new NormalizedPath("Home.cshtml"))
                .WithModel(Config.FromDocument((document, _) => document.GetObject<HomeViewModel>())),
            new SetDestination(Config.FromDocument((_, _) => new NormalizedPath("index.html")))
        ];

        OutputModules =
        [
            new WriteFiles()
        ];
    }
}