using System.ComponentModel.DataAnnotations;
using IMilosk.WebParsingUtils;

namespace ArticleAggregator.Settings;

public class ScrapingSettings
{
    [Required] public List<XPathConfig> XPathConfigs { get; init; } = [];
}

public class XPathConfig
{
    [Required] public required Uri BaseUrl { get; init; }
    [Required] public string TitleXPath { get; init; } = string.Empty;
    [Required] public string SummaryXPath { get; init; } = string.Empty;
    [Required] public string LinkXPath { get; init; } = string.Empty;
    [Required] public string AuthorXPath { get; init; } = string.Empty;
    [Required] public string PublishDateXPath { get; init; } = string.Empty;
    [Required] public string UpdateDateXPath { get; init; } = string.Empty;
    public bool IsJs { get; init; } = false;
    [Required] public List<Navigation> Navigation { get; init; } = [];
}