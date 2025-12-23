using EstateAgency.Domain.Entities;

namespace EstateAgency.ContractGenerator.Services;

/// <summary>
/// Интерфейс для публикации сообщений в NATS 
/// </summary>
public interface INatsPublisher : IAsyncDisposable
{
    /// <summary>
    /// Попытаться установить подключение к серверу NATS
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public Task<bool> TryConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Ожидание установки подключения к NATS в течение указанного времени
    /// </summary>
    /// <param name="timeout">Максимальное время ожидания подключения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public Task<bool> WaitForConnectionAsync(TimeSpan timeout, CancellationToken cancellationToken = default);

    /// <summary>
    /// Публикация запроса на создание контракта в NATS
    /// </summary>
    /// <param name="request">Запрос на создание контракта</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public Task PublishRequestAsync(Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Флаг, указывающий на состояние подключения к NATS
    /// </summary>
    public bool IsConnected { get; }
}