using ArticleAggregator.Core.Parsers.Implementations;
using ArticleAggregator.Core.Parsers.Interfaces;
using ArticleAggregator.Core.Repositories.Implementations;
using ArticleAggregator.Settings;
using Common.BaseTypeExtensions;
using Common.Data.Dapper.Utils.Mappers;
using Common.Data.SqlClient;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace ArticleAggregator.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddArticleAggregator(
        this IServiceCollection serviceCollection,
        IConfigurationRoot configuration
    )
    {
        serviceCollection
            .AddAndValidateServiceOptions<DatabaseSettings>(configuration)
            .AddAndValidateServiceOptions<RssFeedSettings>(configuration)
            .AddAndValidateServiceOptions<ScrapingSettings>(configuration);

        serviceCollection.AddScoped<QueryFactory>(x =>
        {
            var databaseConnector = x.GetRequiredService<IDatabaseConnector>();
            var connectionString = databaseConnector.GetConnection().ConnectionString;

            var connection = new SqliteConnection(connectionString);
            var compiler = new SqliteCompiler();

            return new QueryFactory(connection, compiler);
        });

        serviceCollection
            .AddScoped<IDatabaseConnector, DatabaseConnector>()
            .AddScoped<ISqlTransactionManager, SqlTransactionManager>()
            .AddScoped<IRssFeedParser, RssFeedParser>()
            .AddScoped<IXPathFeedParser, XPathFeedParser>()
            .AddScoped<ArticleRepository>()
            ;

        SqlMapper.RemoveTypeMap(typeof(Uri));
        SqlMapper.AddTypeHandler(new SqlUriTypeHandler());

        return serviceCollection;
    }
}