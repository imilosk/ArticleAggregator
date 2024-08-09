using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.BlogGenerator.Modules;
using ArticleAggregator.BlogGenerator.ViewModels;
using ArticleAggregator.Core.DataModels;
using Statiq.Razor;

namespace ArticleAggregator.BlogGenerator.Pipelines;

public class HomePipeline : Pipeline
{
    public HomePipeline(LoadArticlesModule loadArticlesModule)
    {
        InputModules =
        [
            loadArticlesModule,
        ];

        ProcessModules =
        [
            // new PaginateDocuments(20),
            new ExecuteConfig(
                Config.FromContext(executionContext =>
                {
                    // var input

                    var articles = executionContext.Inputs.GetObjects<Article>().ToList();

                    var pageNumber = 1;
                    const int perPage = 2;
                    var documents = new List<IDocument>();
                    for (var i = 0; i < articles.Count; i += perPage)
                    {
                        var perPageArticles = articles.Skip(i).Take(2);

                        var homeViewModel = new HomeViewModel
                        {
                            Articles = perPageArticles,
                            CurrentPage = pageNumber,
                            PreviousPage = pageNumber + 1,
                            NextPage = pageNumber - 1,
                        };

                        documents.Add(homeViewModel.ToDocument(executionContext));
                        pageNumber++;
                    }

                    return documents;
                })
            ),
            new RenderRazor()
                .WithLayout(new NormalizedPath("Home.cshtml"))
                .WithModel(Config.FromDocument((document, _) => document.GetObject<HomeViewModel>())),
            new SetDestination(Config.FromDocument((document, _) =>
            {
                var model = document.GetObject<HomeViewModel>();

                return model.CurrentPage == 1
                    ? new NormalizedPath("index.html")
                    : new NormalizedPath($"{model.CurrentPage}.html");
            }))
        ];

        OutputModules =
        [
            new WriteFiles()
        ];
    }
}