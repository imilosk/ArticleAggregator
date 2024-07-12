namespace ArticleAggregator.Core.DataModels.Responses;

public class ArticleResponse
{
    public long Id { get; set; }
    public string Title { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Link { get; init; } = string.Empty;
    public DateTime PublishDate { get; init; }
    public DateTime LastUpdatedTime { get; init; }
}