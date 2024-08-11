using System.ComponentModel.DataAnnotations;

namespace ArticleAggregator.Settings;

public class RssFeedSettings
{
    [Required] public List<RssFeedConfig> FeedConfigs { get; init; } = [];
}

public class RssFeedConfig
{
    [Required] public string BaseUrl { get; init; } = string.Empty;
    [Required] public string FallbackAuthor { get; init; } = string.Empty;
}