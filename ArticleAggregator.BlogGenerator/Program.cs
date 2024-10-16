using ArticleAggregator.BlogGenerator.Modules;
using ArticleAggregator.BlogGenerator.Pipelines;
using ArticleAggregator.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddEnvironmentVariables()
    .Build();

await Bootstrapper
    .Factory
    .CreateDefault(args)
    .AddHostingCommands()
    .ConfigureServices(services =>
    {
        services
            .AddArticleAggregator(configuration)
            .AddScoped<LoadArticlesModule>();
    })
    .AddPipeline<SitemapPipeline>(nameof(SitemapPipeline))
    .AddPipeline<ArticlesPipeline>(nameof(ArticlesPipeline))
    .RunAsync();