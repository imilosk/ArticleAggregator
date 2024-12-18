using ArticleAggregator.Core.Extensions;
using ArticleAggregator.DataIngest.Jobs;
using ArticleAggregator.DataIngest.Services.Implementations;
using ArticleAggregator.DataIngest.Services.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

var environment = builder.Environment.EnvironmentName;
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddArticleAggregator(configuration);

builder.Services
    .AddScoped<IDataIngestService, DataIngestService>()
    .AddScoped<DataIngestJob>();

var host = builder.Build();

var scope = host.Services.CreateScope();
var job = scope.ServiceProvider.GetRequiredService<DataIngestJob>();

await job.ExecuteAsync();