using EstateAgency.ContractGenerator.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstateAgency.ContractGenerator.Services;

/// <summary>
/// Фоновый сервис для генерации и публикации контрактов в NATS
/// </summary>
/// <param name="natsPublisher">Публикатор для отправки сообщений в NATS</param>
/// <param name="requestGenerator">Генератор запросов на создание контрактов</param>
/// <param name="options">Опции настройки генератора</param>
/// <param name="logger">Логгер для записи событий</param>
public class ContractGenerationService(
    INatsPublisher natsPublisher,
    IRequestGenerator requestGenerator,
    IOptions<GeneratorOptions> options,
    ILogger<ContractGenerationService> logger) : BackgroundService
{
    private readonly INatsPublisher _natsPublisher = natsPublisher;
    private readonly IRequestGenerator _requestGenerator = requestGenerator;
    private readonly GeneratorOptions _options = options.Value;
    private readonly ILogger<ContractGenerationService> _logger = logger;

    /// <summary>
    /// Основной метод выполнения фоновой задачи
    /// </summary>
    /// <param name="stoppingToken">Токен отмены для остановки сервиса</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Contract Generation Service starting");
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        _logger.LogInformation("Configuration: Interval={IntervalMs}ms, BatchSize={BatchSize}",
            _options.IntervalMs, _options.BatchSize);

        if (!await _natsPublisher.WaitForConnectionAsync(TimeSpan.FromSeconds(30), stoppingToken))
        {
            _logger.LogCritical("Failed to establish NATS connection after all attempts");
            return;
        }

        _logger.LogInformation("Starting contract generation loop...");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessBatchAsync(stoppingToken);

                    if (!stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(_options.IntervalMs, stoppingToken);
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during batch processing");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
        finally
        {
            _logger.LogInformation("Stopping Contract Generation Service");
            await _natsPublisher.DisposeAsync();
        }
    }

    /// <summary>
    /// Обработка одного пакета запросов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    private async Task ProcessBatchAsync(CancellationToken cancellationToken)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var batch = await _requestGenerator.GenerateBatchAsync(_options.BatchSize, cancellationToken);
        var batchList = batch.ToList();

        _logger.LogInformation("Generated {Count} requests in {ElapsedMs}ms",
            batchList.Count, stopwatch.ElapsedMilliseconds);

        var sentCount = 0;
        var tasks = new List<Task>();

        foreach (var request in batchList)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            var task = Task.Run(async () =>
            {
                try
                {
                    await _natsPublisher.PublishRequestAsync(request, cancellationToken);
                    Interlocked.Increment(ref sentCount);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to publish request #{RequestId}", request.Id);
                }
            }, cancellationToken);

            tasks.Add(task);

            if (tasks.Count >= 5)
            {
                await Task.WhenAll(tasks);
                tasks.Clear();
            }
        }

        if (tasks.Count != 0)
        {
            await Task.WhenAll(tasks);
        }

        stopwatch.Stop();

        _logger.LogInformation("Published {SentCount}/{TotalCount} requests in {TotalElapsedMs}ms",
            sentCount, batchList.Count, stopwatch.ElapsedMilliseconds);
    }
}