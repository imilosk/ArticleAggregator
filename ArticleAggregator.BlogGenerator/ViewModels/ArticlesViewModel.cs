using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.ViewModels;

public class ArticlesViewModel
{
    public IEnumerable<Article> Articles { get; init; } = [];
    public int CurrentPage { get; init; }
    public int PreviousPage { get; init; }
    public int NextPage { get; init; }
    public object CurrentPageUri { get; init; } = string.Empty;
    public string PreviousPageUri { get; init; } = string.Empty;
    public string NextPageUri { get; init; } = string.Empty;
}