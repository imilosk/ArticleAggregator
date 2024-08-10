namespace ArticleAggregator.BlogGenerator.ClassExtensions;

internal static class DocumentToObjectExtensions
{
    public static T GetObject<T>(this IDocument document)
    {
        return ((ObjectDocument<T>)document).Object;
    }

    public static IEnumerable<IDocument> ToDocuments<T>(this IEnumerable<T> items, IExecutionContext executionContext)
    {
        return items.Select(item => item.ToDocument(executionContext));
    }

    public static IEnumerable<T> GetObjects<T>(this IEnumerable<IDocument> items)
    {
        return items.Select(item => item.GetObject<T>());
    }
}