namespace EstateAgency.Application.DTOs.Analytics;

/// <summary>
/// Содержит агрегированные данные по продажам за указанный временной интервал
/// </summary>
public class SellerPeriodDto
{
    /// <summary>
    /// Идентификатор клиента-продавца
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Полное имя клиента-продавца
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Количество совершенных продаж за период
    /// </summary>
    public int SalesCount { get; set; }

    /// <summary>
    /// Суммарная выручка от продаж за период
    /// </summary>
    public decimal TotalSalesAmount { get; set; }

    /// <summary>
    /// Начальная дата анализируемого периода
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// Конечная дата анализируемого периода
    /// </summary>
    public DateTime PeriodEnd { get; set; }
}