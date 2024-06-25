using FluentMigrator;

namespace ArticleAggregator.DbMigrations.Migrations;

[Migration(202406252237)]
public class AddArticlesTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Articles")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Title").AsString()
            .WithColumn("Summary").AsString()
            .WithColumn("Author").AsString()
            .WithColumn("PublishDate").AsDateTime2()
            .WithColumn("LastUpdatedTime").AsDateTime2();
    }
}