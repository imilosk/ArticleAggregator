using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleAggregator.Core.DataModels;

public record Article
{
    [NotMapped] public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public required Uri Link { get; init; }
    public DateTime PublishDate { get; set; }
    public DateTime LastUpdatedTime { get; set; }
}