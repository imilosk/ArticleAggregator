using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleAggregator.Core.DataModels;

public record Article
{
    [NotMapped] public long Id;
    public string Title = string.Empty;
    public string Summary = string.Empty;
    public string Author = string.Empty;
    public required Uri Link;
    public DateTime PublishDate;
    public DateTime LastUpdatedTime;
}