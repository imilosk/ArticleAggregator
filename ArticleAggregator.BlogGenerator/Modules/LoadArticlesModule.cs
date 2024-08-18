using ArticleAggregator.BlogGenerator.ClassExtensions;
using ArticleAggregator.Core.Repositories.Implementations;

namespace ArticleAggregator.BlogGenerator.Modules;

internal class LoadArticlesModule : IModule
{
    private readonly ArticleRepository _articleRepository;

    public LoadArticlesModule(ArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<IEnumerable<IDocument>> ExecuteAsync(IExecutionContext executionContext)
    {
        var articles = await _articleRepository.GetAll();

        return articles
            .OrderByDescending(x => x.PublishDate)
            .ToDocuments(executionContext);
    }
}