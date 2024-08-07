using ArticleAggregator.BlogGenerator.CustomExtensions;
using ArticleAggregator.BlogGenerator.Modules;
using ArticleAggregator.BlogGenerator.ViewModels;
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
                Config.FromContext(context =>
                {
                    var articles = context.Inputs.Select(document =>
                        document.GetArticle()
                    );

                    var homeViewModel = new HomeViewModel
                    {
                        Articles = articles,
                    };

                    return homeViewModel.ToIDocument(context).Yield();
                })
            ),
            new RenderRazor()
                .WithLayout(new NormalizedPath("Home.cshtml"))
                .WithModel(Config.FromDocument((document, _) => document.GetHomeViewModel())),
            new SetDestination(Config.FromDocument((_, _) => new NormalizedPath("index.html")))
        ];

        OutputModules =
        [
            new WriteFiles()
        ];
    }
}