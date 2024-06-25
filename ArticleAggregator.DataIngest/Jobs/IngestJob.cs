using ArticleAggregator.DataIngest.Services.Interfaces;

namespace ArticleAggregator.DataIngest.Jobs;

public class IngestJob
{
    private readonly ILogger<IngestJob> _logger;
    private readonly IDataIngestService _dataIngestService;

    public IngestJob(ILogger<IngestJob> logger, IDataIngestService dataIngestService)
    {
        _logger = logger;
        _dataIngestService = dataIngestService;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Job running at: {time}", DateTimeOffset.UtcNow);

        await _dataIngestService.Ingest();

        _logger.LogInformation("Job finishing at: {time}", DateTimeOffset.UtcNow);
    }
}