using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.BlogGenerator.CustomExtensions;

public static class ArticleExtensions
{
    public static IDocument ToIDocument(this Article article, IExecutionContext executionContext)
    {
        return executionContext.CreateDocument(
            new MetadataItems
            {
                { Constants.Article.MetadataKey, article }
            });
    }
}