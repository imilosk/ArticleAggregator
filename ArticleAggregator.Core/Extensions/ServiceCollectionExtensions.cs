using ArticleAggregator.Core.Parsers.Implementations;
using ArticleAggregator.Core.Parsers.Interfaces;
using ArticleAggregator.Core.Repositories.Implementations;
using ArticleAggregator.Core.Repositories.Interfaces;
using ArticleAggregator.Settings;
using Common.BaseTypeExtensions;
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
            .AddScoped<IXPathFeedParser, XPathFeedParser>();

        serviceCollection
            .AddScoped<IArticleRepository, ArticleRepository>();

        return serviceCollection;
    }
}