namespace EstateAgency.Application.DTOs.Analytics;

/// <summary>
/// DTO для аналитики по клиентам
/// Используется для передачи аналитической информации в API responses
/// </summary>
public class ClientAnalyticsDto
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Полное имя клиента
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Номер телефона клиента
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Общее количество заявок клиента
    /// </summary>
    public int RequestsCount { get; set; }

    /// <summary>
    /// Тип заявки (покупка/продажа)
    /// </summary>
    public string? RequestType { get; set; }

    /// <summary>
    /// Целевой тип недвижимости, которую ищет клиент
    /// </summary>
    public string? TargetPropertyType { get; set; }

    /// <summary>
    /// Минимальная сумма в заявках клиента
    /// </summary>
    public decimal? MinAmount { get; set; }

    /// <summary>
    /// Ранг клиента в рейтинге
    /// </summary>
    public int? Rank { get; set; }
}