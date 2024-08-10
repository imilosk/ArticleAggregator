using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.BlogGenerator.ViewModels;
using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.Modules;

internal class ArticlePaginationModule : IModule
{
    public Task<IEnumerable<IDocument>> ExecuteAsync(IExecutionContext executionContext)
    {
        var numberOfPages = executionContext.Inputs.Length;

        var documents = new List<IDocument>();
        var currentPage = 1;

        foreach (var document in executionContext.Inputs)
        {
            var articles = document.GetChildren().GetObjects<Article>();
            var previousPage = currentPage - 1;
            var nextPage = currentPage + 1;

            var homeViewModel = new ArticlesViewModel
            {
                Articles = articles,
                CurrentPage = currentPage,
                PreviousPage = previousPage,
                NextPage = nextPage,
                CurrentPageUri = currentPage switch
                {
                    1 => "index.html",
                    _ => $"{currentPage}.html"
                },
                PreviousPageUri = previousPage switch
                {
                    0 => string.Empty,
                    1 => "index.html",
                    _ => $"{previousPage}.html"
                },
                NextPageUri = nextPage > numberOfPages ? string.Empty : $"{nextPage}.html",
            };

            documents.Add(homeViewModel.ToDocument(executionContext));
            currentPage++;
        }

        return Task.FromResult<IEnumerable<IDocument>>(documents);
    }
}