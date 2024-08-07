using System.Globalization;
using ArticleAggregator.BlogGenerator.ViewModels;
using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.CustomExtensions;

public static class DocumentExtensions
{
    public static ArticleViewModel ToArticleViewModel(this IDocument document)
    {
        return new ArticleViewModel
        {
            Id = document.GetValueOrDefault(
                $"{nameof(Article)}.{nameof(Article.Id)}",
                (long)0
            ),
            Title = document.GetValueOrDefault(
                $"{nameof(Article)}.{nameof(Article.Title)}",
                string.Empty
            ),
            Summary = document.GetValueOrDefault(
                $"{nameof(Article)}.{nameof(Article.Summary)}",
                string.Empty
            ),
            Author = document.GetValueOrDefault(
                $"{nameof(Article)}.{nameof(Article.Author)}",
                string.Empty
            ),
            Link = document.GetValueOrDefault(
                $"{nameof(Article)}.{nameof(Article.Link)}",
                string.Empty
            ),
            PublishDate = DateTime.Parse(
                document.GetValueOrDefault(
                    $"{nameof(Article)}.{nameof(Article.PublishDate)}",
                    string.Empty
                )),
            LastUpdatedTime = DateTime.Parse(
                document.GetValueOrDefault(
                    $"{nameof(Article)}.{nameof(Article.LastUpdatedTime)}",
                    string.Empty
                )),
            Source = document.GetValueOrDefault(
                $"{nameof(Article)}.{nameof(Article.Source)}",
                string.Empty
            ),
        };
    }

    public static T GetValueOrDefault<T>(this IDocument document, string key, T defaultValue)
        where T : ISpanParsable<T>
    {
        return T.TryParse(document.GetString(key), CultureInfo.InvariantCulture, out var value)
            ? value
            : defaultValue;
    }
}