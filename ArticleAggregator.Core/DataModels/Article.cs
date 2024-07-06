using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleAggregator.Core.DataModels;

public record Article
{
    [NotMapped] public long Id { get; set; }
    public string Title { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public required Uri Link { get; init; }
    public DateTime PublishDate { get; init; }
    public DateTime LastUpdatedTime { get; init; }
}