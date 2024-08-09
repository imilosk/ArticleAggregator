using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Article> Articles { get; init; } = [];
    public int CurrentPage { get; init; }
    public int PreviousPage { get; init; }
    public int NextPage { get; init; }
}