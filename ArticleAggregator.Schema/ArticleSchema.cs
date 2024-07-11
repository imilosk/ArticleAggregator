namespace ArticleAggregator.Schema;

public static class ArticleSchema
{
    public const string TableName = "Article";

    public static class Columns
    {
        public const string Id = nameof(Id);
        public const string Title = nameof(Title);
        public const string Summary = nameof(Summary);
        public const string Author = nameof(Author);
        public const string Link = nameof(Link);
        public const string PublishDate = nameof(PublishDate);
        public const string LastUpdatedTime = nameof(LastUpdatedTime);
        public const string Source = nameof(Source);
    }
}