using EstateAgency.Application.DTOs.Analytics;
using EstateAgency.Domain.Enum;

namespace EstateAgency.Application.Interfaces.Services;

/// <summary>
/// Интерфейс сервиса для аналитики и отчетности 
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Получает продавцов и их заявки за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param> 
    public Task<List<SellerPeriodDto>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получает топ клиентов по количеству заявок указанного типа
    /// </summary>
    /// <param name="type">Тип заявки (покупка/продажа)</param>
    /// <param name="topCount">Количество клиентов в топе (по умолчанию 5)</param> 
    public Task<List<ClientAnalyticsDto>> GetTopClientsByRequestsAsync(RequestType type, int topCount = 5);

    /// <summary>
    /// Получает статистику количества заявок по типам недвижимости
    /// </summary> 
    public Task<List<PropertyTypeStatDto>> GetRequestCountByPropertyTypeAsync();

    /// <summary>
    /// Получает клиентов с заявками минимальной стоимости
    /// </summary> 
    public Task<List<ClientAnalyticsDto>> GetClientsWithMinAmountRequestAsync();

    /// <summary>
    /// Получает клиентов, ищущих недвижимость заданного типа
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для поиска</param> 
    public Task<List<ClientAnalyticsDto>> GetClientsSearchingPropertyTypeAsync(PropertyType propertyType);
}