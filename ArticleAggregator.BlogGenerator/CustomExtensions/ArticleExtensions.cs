using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.CustomExtensions;

public static class ArticleExtensions
{
    public static IDocument ToIDocument(this Article article, IExecutionContext executionContext)
    {
        var item = new Dictionary<string, object>
        {
            { $"{nameof(Article)}.{nameof(Article.Id)}", article.Id },
            { $"{nameof(Article)}.{nameof(Article.Title)}", article.Title },
            { $"{nameof(Article)}.{nameof(Article.Summary)}", article.Summary },
            { $"{nameof(Article)}.{nameof(Article.Author)}", article.Author },
            { $"{nameof(Article)}.{nameof(Article.Link)}", article.Link.ToString() },
            { $"{nameof(Article)}.{nameof(Article.PublishDate)}", article.PublishDate },
            { $"{nameof(Article)}.{nameof(Article.LastUpdatedTime)}", article.LastUpdatedTime },
            { $"{nameof(Article)}.{nameof(Article.Source)}", article.Source }
        };

        return executionContext.CreateDocument(item);
    }
}