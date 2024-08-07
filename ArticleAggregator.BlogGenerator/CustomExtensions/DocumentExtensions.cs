using ArticleAggregator.BlogGenerator.ViewModels;
using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.CustomExtensions;

public static class DocumentExtensions
{
    public static Article GetArticle(this IDocument document)
    {
        return document.Get<Article>(Constants.Article.MetadataKey);
    }

    public static HomeViewModel GetHomeViewModel(this IDocument document)
    {
        return document.Get<HomeViewModel>(Constants.Home.MetadataKey);
    }
}