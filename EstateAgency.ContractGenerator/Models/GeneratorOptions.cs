namespace EstateAgency.ContractGenerator.Models;

/// <summary>
/// Опции для настройки генератора контрактов
/// </summary>
public class GeneratorOptions
{
    /// <summary>
    /// Интервал между операциями генерации в миллисекундах
    /// </summary>
    public int IntervalMs { get; set; } = 5000;

    /// <summary>
    /// Размер пакета для обработки контрактов
    /// </summary>
    public int BatchSize { get; set; } = 10;

    /// <summary>
    /// Включение потоковой обработки контрактов
    /// </summary>
    public bool EnableStreaming { get; set; } = true;

    /// <summary>
    /// Максимальное количество одновременных публикаций контрактов
    /// Значение по умолчанию: 5
    /// </summary>
    public int MaxConcurrentPublishes { get; set; } = 5;
}