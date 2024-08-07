using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Article> Articles { get; init; } = [];
}