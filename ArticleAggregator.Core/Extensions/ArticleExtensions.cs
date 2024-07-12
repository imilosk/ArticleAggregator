using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.DataModels.Responses;

namespace ArticleAggregator.Core.Extensions;

public static class ArticleExtensions
{
    public static ArticleResponse MapToResponse(this Article article)
    {
        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Summary = article.Summary,
            Author = article.Author,
            Link = article.Link.ToString(),
            PublishDate = article.PublishDate,
            LastUpdatedTime = article.LastUpdatedTime,
        };
    }

    public static IEnumerable<ArticleResponse> MapToResponse(this IEnumerable<Article> articles)
    {
        return articles.Select(article => article.MapToResponse());
    }
}