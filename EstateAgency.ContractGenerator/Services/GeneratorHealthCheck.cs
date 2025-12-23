using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace EstateAgency.ContractGenerator.Services;

/// <summary>
/// Проверка работоспособности сервиса генератора контрактов
/// </summary>
/// <param name="natsPublisher">Публикатор NATS для проверки подключения</param>
/// <param name="logger">Логгер для записи событий проверки работоспособности</param>
public class GeneratorHealthCheck(INatsPublisher natsPublisher, ILogger<GeneratorHealthCheck> logger) : IHealthCheck
{
    private readonly INatsPublisher _natsPublisher = natsPublisher;
    private readonly ILogger<GeneratorHealthCheck> _logger = logger;

    /// <summary>
    /// Выполняет проверку работоспособности сервиса генератора контрактов
    /// </summary>
    /// <param name="context">Контекст проверки работоспособности</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var data = new Dictionary<string, object>
            {
                ["timestamp"] = DateTime.UtcNow,
                ["service"] = "ContractGenerator"
            };

            if (_natsPublisher.IsConnected)
            {
                _logger.LogDebug("Health check: NATS connection established");
                return HealthCheckResult.Healthy("NATS connection established", data);
            }
            else
            {
                _logger.LogWarning("Health check: NATS connection not established");
                return HealthCheckResult.Unhealthy("NATS connection not established", null, data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during health check");
            return HealthCheckResult.Unhealthy("Health check failed", ex);
        }
    }
}