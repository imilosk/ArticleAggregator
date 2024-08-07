using ArticleAggregator.BlogGenerator.ViewModels;

namespace ArticleAggregator.BlogGenerator.CustomExtensions;

public static class HomeExtensions
{
    public static IDocument ToIDocument(this HomeViewModel homeViewModel, IExecutionContext executionContext)
    {
        return executionContext.CreateDocument(
            new MetadataItems
            {
                { Constants.Home.MetadataKey, homeViewModel }
            });
    }
}