namespace EstateAgency.ContractGenerator.Models;

/// <summary>
/// Настройки для подключения к NATS 
/// </summary>
public class NatsOptions
{
    /// <summary>
    /// URL-адрес сервера NATS
    /// </summary>
    public string Url { get; set; } = "nats://localhost:4222";

    /// <summary>
    /// Количество повторных попыток подключения при сбое
    /// </summary>
    public int RetryCount { get; set; } = 5;

    /// <summary>
    /// Задержка между повторными попытками подключения в миллисекундах
    /// </summary>
    public int RetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Таймаут подключения к серверу NATS в секундах
    /// </summary>
    public int ConnectionTimeoutSeconds { get; set; } = 30;
}