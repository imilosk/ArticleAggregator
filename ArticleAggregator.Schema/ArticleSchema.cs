namespace ArticleAggregator.Schema;

public static class ArticleSchema
{
    public const string TableName = "Article";

    public static class Columns
    {
        public const string Id = "Id";
        public const string Title = "Title";
        public const string Summary = "Summary";
        public const string Author = "Author";
        public const string Link = "Link";
        public const string PublishDate = "PublishDate";
        public const string LastUpdatedTime = "LastUpdatedTime";
    }
}