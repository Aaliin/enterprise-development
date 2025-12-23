using EstateAgency.Domain.Entities;

namespace EstateAgency.ContractGenerator.Services;

/// <summary>
/// Интерфейс для генерации запросов на создание контрактов
/// </summary>
public interface IRequestGenerator
{
    /// <summary>
    /// Генерирует пакет запросов на создание контрактов
    /// </summary>
    /// <param name="count">Количество запросов для генерации</param>
    public IEnumerable<Request> GenerateBatch(int count);

    /// <summary>
    /// Генерирует одиночный запрос на создание контракта
    /// </summary>
    public Request GenerateSingle();

    /// <summary>
    /// Асинхронно генерирует пакет запросов на создание контрактов
    /// </summary>
    /// <param name="count">Количество запросов для генерации</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public Task<IEnumerable<Request>> GenerateBatchAsync(int count, CancellationToken cancellationToken = default);
}