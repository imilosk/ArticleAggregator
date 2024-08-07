using ArticleAggregator.BlogGenerator.Pipelines;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddEnvironmentVariables()
    .Build();

return await Bootstrapper
    .Factory
    .CreateDefault(args)
    .AddHostingCommands()
    // .ConfigureServices(services =>
    //     services.SetupArticleAggregatorDependencyInject(configuration)
    // )
    .AddPipeline<HomePipeline>()
    .RunAsync();