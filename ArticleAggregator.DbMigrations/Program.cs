using ArticleAggregator.DbMigrations.Migrations;
using Common.BaseTypeExtensions;
using Common.Data.SqlClient;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSQLite()
        .WithGlobalConnectionString(x =>
        {
            var databaseConnector = x.GetRequiredService<IDatabaseConnector>();

            return databaseConnector.GetConnection().ConnectionString;
        })
        .ScanIn(typeof(AddArticlesTable).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

services.AddScoped<IDatabaseConnector, DatabaseConnector>();
services.AddAndValidateServiceOptions<DatabaseSettings>(configuration);

using var serviceProvider = services.BuildServiceProvider(false);

using var scope = serviceProvider.CreateScope();
var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

runner.MigrateUp();