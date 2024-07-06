using ArticleAggregator.Schema;
using FluentMigrator;

namespace ArticleAggregator.DbMigrations.Migrations;

[Migration(202406252237)]
public class AddArticlesTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table(ArticleSchema.TableName)
            .WithColumn(ArticleSchema.Columns.Id).AsInt64().PrimaryKey().Identity()
            .WithColumn(ArticleSchema.Columns.Title).AsString()
            .WithColumn(ArticleSchema.Columns.Summary).AsString()
            .WithColumn(ArticleSchema.Columns.Author).AsString()
            .WithColumn(ArticleSchema.Columns.Link).AsString()
            .WithColumn(ArticleSchema.Columns.PublishDate).AsDateTime2()
            .WithColumn(ArticleSchema.Columns.LastUpdatedTime).AsDateTime2();
    }
}