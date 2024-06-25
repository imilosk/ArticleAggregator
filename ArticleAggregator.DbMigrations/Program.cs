using ArticleAggregator.DbMigrations.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

using var serviceProvider = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSQLite()
        .WithGlobalConnectionString("Data Source=article_aggregator.db")
        .ScanIn(typeof(AddArticlesTable).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole())
    .BuildServiceProvider(false);

using var scope = serviceProvider.CreateScope();
var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

runner.MigrateUp();