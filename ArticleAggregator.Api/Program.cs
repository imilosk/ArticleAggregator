using ArticleAggregator.Core.Extensions;
using ArticleAggregator.Core.Repositories.Interfaces;
using Common.Data.SqlKata.Utils.Filtering;

var builder = WebApplication.CreateSlimBuilder(args);

var environment = builder.Environment.EnvironmentName;
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddArticleAggregator(configuration);

var app = builder.Build();

app.MapGet("/api/articles",
    async (IArticleRepository articleRepository, int page = 1, int pageSize = 100) =>
    {
        if (page < 1)
        {
            return Results.BadRequest("Page number must be greater than 0.");
        }

        if (pageSize is < 1 or > 1000)
        {
            return Results.BadRequest("Page size must be between 1 and 1000.");
        }

        var filterBuilder = new ComparisonFilterBuilder()
            .AddFilter(FilterOperator.Offset, (page - 1) * pageSize)
            .AddFilter(FilterOperator.Limit, pageSize)
            .Build();
        var article = await articleRepository.GetAll(filterBuilder);

        return Results.Ok(article.MapToResponse());
    });

app.Run();