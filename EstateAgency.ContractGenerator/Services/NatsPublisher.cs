using EstateAgency.ContractGenerator.Models;
using EstateAgency.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace EstateAgency.ContractGenerator.Services;

/// <summary>
/// Реализация публикатора NATS для отправки сообщений в брокер сообщений
/// </summary>
public class NatsPublisher : INatsPublisher
{
    private readonly NatsOptions _options;
    private readonly ILogger<NatsPublisher> _logger;
    private NatsConnection? _connection;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly AsyncRetryPolicy _connectionRetryPolicy;
    private readonly AsyncRetryPolicy _publishRetryPolicy;

    public bool IsConnected => _connection != null && _connection.ConnectionState == NatsConnectionState.Open;

    /// <summary>
    /// Конструктор публикатора NATS
    /// </summary>
    /// <param name="options">Опции подключения к NATS</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <param name="logger">Логгер для записи событий</param>
    public NatsPublisher(
        IOptions<NatsOptions> options,
        IConfiguration configuration,
        ILogger<NatsPublisher> logger)
    {
        _options = options.Value;
        _configuration = configuration;
        _logger = logger;

        LogConfigurationSources();

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        _connectionRetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                _options.RetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(_options.RetryDelayMs * Math.Pow(2, retryAttempt - 1)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception,
                        "Retry {RetryCount}/{MaxRetries} for connection after {Delay}ms",
                        retryCount, _options.RetryCount, timeSpan.TotalMilliseconds);
                });

        _publishRetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromMilliseconds(500 * retryAttempt),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception,
                        "Retry {RetryCount} for publishing after {Delay}ms",
                        retryCount, timeSpan.TotalMilliseconds);
                });
    }

    /// <summary>
    /// Записывает в лог информацию об источниках конфигурации для отладки
    /// </summary>
    private void LogConfigurationSources()
    {
        _logger.LogInformation("NATS CONFIGURATION DEBUG");

        var connectionStringNats = _configuration.GetConnectionString("Nats");
        var natsUrlFromConfig = _options.Url;
        var envConnectionStrings = Environment.GetEnvironmentVariable("ConnectionStrings__Nats");
        var envNatsUrl = Environment.GetEnvironmentVariable("NATS_URL");

        _logger.LogInformation("1. _configuration.GetConnectionString('Nats'): {Value}",
            connectionStringNats ?? "(null)");
        _logger.LogInformation("2. _options.Url: {Value}",
            natsUrlFromConfig ?? "(null)");
        _logger.LogInformation("3. ENV ConnectionStrings__Nats: {Value}",
            envConnectionStrings ?? "(null)");
        _logger.LogInformation("4. ENV NATS_URL: {Value}",
            envNatsUrl ?? "(null)");

        var finalUrl = GetNatsUrlWithPriority();
        _logger.LogInformation("FINAL URL: {Url}", finalUrl);
        _logger.LogInformation("=================================");
    }

    /// <summary>
    /// Определяет URL для подключения к NATS 
    /// </summary>
    private string GetNatsUrlWithPriority()
    {
        var sources = new Dictionary<string, Func<string?>>
        {
            ["Environment Variable ConnectionStrings__Nats"] = () =>
                Environment.GetEnvironmentVariable("ConnectionStrings__Nats"),
            ["Environment Variable NATS_URL"] = () =>
                Environment.GetEnvironmentVariable("NATS_URL"),
            ["Configuration ConnectionStrings:Nats"] = () =>
                _configuration.GetConnectionString("Nats"),
            ["Configuration Nats:Url"] = () => _options.Url
        };

        foreach (var source in sources)
        {
            var url = source.Value();
            if (!string.IsNullOrEmpty(url))
            {
                _logger.LogInformation("Using NATS URL from {Source}: {Url}", source.Key, url);
                return url;
            }
        }

        var defaultUrl = "nats://localhost:4222";
        _logger.LogInformation("Using default NATS URL: {Url}", defaultUrl);
        return defaultUrl;
    }

    /// <summary>
    /// Пытается установить подключение к серверу NATS
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public async Task<bool> TryConnectAsync(CancellationToken cancellationToken = default)
    {
        var natsUrl = GetNatsUrlWithPriority();
        _logger.LogInformation("Connecting to NATS at {Url}", natsUrl);

        return await _connectionRetryPolicy.ExecuteAsync(async () =>
        {
            try
            {
                var opts = new NatsOpts
                {
                    Url = natsUrl,
                    ConnectTimeout = TimeSpan.FromSeconds(10),
                    ReconnectWaitMax = TimeSpan.FromSeconds(30),
                    Name = "EstateAgency.Generator",
                    Echo = false,
                    Verbose = true
                };

                _connection = new NatsConnection(opts);
                await _connection.ConnectAsync();

                _logger.LogInformation("Successfully connected to NATS");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to NATS");
                throw;
            }
        });
    }
    /// <summary>
    /// Ожидает установку подключения к NATS в течение указанного времени
    /// </summary>
    /// <param name="timeout">Максимальное время ожидания подключения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public async Task<bool> WaitForConnectionAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Waiting for NATS connection (timeout: {Timeout}s)...", timeout.TotalSeconds);

        var startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < timeout)
        {
            if (IsConnected)
            {
                _logger.LogInformation("NATS connection established");
                return true;
            }

            if (!await TryConnectAsync(cancellationToken))
            {
                await Task.Delay(1000, cancellationToken);
            }
        }

        _logger.LogError("Timeout waiting for NATS connection");
        return false;
    }

    /// <summary>
    /// Публикует запрос на создание контракта в NATS
    /// </summary>
    /// <param name="request">Запрос на создание контракта</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public async Task PublishRequestAsync(Request request, CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("NATS connection is not established");
        }

        await _publishRetryPolicy.ExecuteAsync(async () =>
        {
            try
            {
                var requestDto = new
                {
                    request.Id,
                    request.ClientId,
                    request.PropertyId,
                    request.Type,
                    request.Amount,
                    request.CreatedDate,
                    Client = request.Client != null ? new
                    {
                        request.Client.Id,
                        request.Client.FullName,
                        request.Client.PassportNumber,
                        request.Client.PhoneNumber
                    } : null,
                    Property = request.Property != null ? new
                    {
                        request.Property.Id,
                        request.Property.Type,
                        request.Property.Purpose,
                        request.Property.CadastralNumber,
                        request.Property.Address,
                        request.Property.Floors,
                        request.Property.TotalArea,
                        request.Property.Rooms,
                        request.Property.CeilingHeight,
                        request.Property.Floor,
                        request.Property.HasEncumbrances
                    } : null
                };

                var json = JsonSerializer.Serialize(requestDto, _jsonOptions);

                await _connection!.PublishAsync(
                    subject: "estateagency.requests.create",
                    data: json,
                    cancellationToken: cancellationToken);

                _logger.LogDebug("Published request #{RequestId}", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish request #{RequestId}", request.Id);
                throw;
            }
        });
    }

    /// <summary>
    /// Асинхронно освобождает ресурсы публикатора NATS
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
                _logger.LogDebug("NATS connection disposed asynchronously");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error during async NATS disconnect");
        }
        finally
        {
            _connection = null;
        }

        GC.SuppressFinalize(this);
    }
}