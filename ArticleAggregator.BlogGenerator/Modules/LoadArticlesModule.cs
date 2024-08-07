using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.Modules;

public class LoadArticlesModule : IModule
{
    private static readonly List<Article> Articles =
    [
        new Article
        {
            Id = 1,
            Title = "First Article",
            Summary = "This is the summary of the first article.",
            Author = "Author One",
            Link = new Uri("/articles/1.html", UriKind.Relative),
            PublishDate = new DateTime(2023, 1, 1),
            LastUpdatedTime = new DateTime(2023, 1, 1),
            Source = "Source One"
        },

        new Article
        {
            Id = 2,
            Title = "Second Article",
            Summary = "This is the summary of the second article.",
            Author = "Author Two",
            Link = new Uri("/articles/2.html", UriKind.Relative),
            PublishDate = new DateTime(2023, 2, 1),
            LastUpdatedTime = new DateTime(2023, 2, 1),
            Source = "Source Two"
        }
    ];

    public Task<IEnumerable<IDocument>> ExecuteAsync(IExecutionContext executionContext)
    {
        var documents = Articles.ToDocuments(executionContext);

        return Task.FromResult(documents);
    }
}