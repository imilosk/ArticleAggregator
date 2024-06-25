using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Services.Implementations;
using ArticleAggregator.Core.Services.Interfaces;
using ArticleAggregator.Settings;
using Common.BaseTypeExtensions;
using Common.HtmlParsingTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArticleAggregator.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection SetupArticleAggregatorDependencyInject(
        this IServiceCollection serviceCollection,
        IConfigurationRoot configuration
    )
    {
        serviceCollection
            .AddAndValidateServiceOptions<RssFeedSettings>(configuration)
            .AddAndValidateServiceOptions<ScrapingSettings>(configuration);

        serviceCollection
            .AddScoped<IRssFeedParser, RssFeedParser>()
            .AddScoped<IXPathFeedParser, XPathFeedParser>()
            .AddScoped<HtmlLoop<Article>>();

        return serviceCollection;
    }
}