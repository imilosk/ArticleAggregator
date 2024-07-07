using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleAggregator.Core.DataModels;

public record Article
{
    [NotMapped] public long Id { get; set; }
    public string Title { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Link => LinkUri.ToString();
    [NotMapped] public required Uri LinkUri;
    public DateTime PublishDate { get; init; }
    public DateTime LastUpdatedTime { get; init; }
    public string Source { get; init; } = string.Empty;
}