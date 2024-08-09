using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.Core.Repositories.Interfaces;

namespace ArticleAggregator.BlogGenerator.Modules;

public class LoadArticlesModule : IModule
{
    private readonly IArticleRepository _articleRepository;

    public LoadArticlesModule(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<IEnumerable<IDocument>> ExecuteAsync(IExecutionContext executionContext)
    {
        var articles = await _articleRepository.GetMany(1, 20);

        return articles.ToDocuments(executionContext);
    }
}