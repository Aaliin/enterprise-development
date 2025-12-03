using EstateAgency.Application.Dto;
using EstateAgency.Domain.Enum;

namespace EstateAgency.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с заявками
/// </summary>
public interface IRequestService
{
    /// <summary>
    /// Получает список всех заявок
    /// </summary>
    public Task<List<RequestDto>> GetAllRequestsAsync();

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<RequestDto?> GetRequestByIdAsync(int id);

    /// <summary>
    /// Создает новую заявку
    /// </summary>
    /// <param name="requestDto">DTO для создания заявки</param>
    public Task<RequestDto> CreateRequestAsync(CreateRequestDto requestDto);

    /// <summary>
    /// Обновляет данные заявки
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="requestDto">DTO с обновленными данными</param>
    public Task<RequestDto?> UpdateRequestAsync(int id, CreateRequestDto requestDto);

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<bool> DeleteRequestAsync(int id);

    /// <summary>
    /// Получает продавцов за заданный период
    /// </summary>
    public Task<List<ClientDto>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получает топ покупателей по количеству заявок
    /// </summary>
    public Task<List<ClientDto>> GetTopBuyersAsync(int topCount = 5);

    /// <summary>
    /// Получает топ продавцов по количеству заявок
    /// </summary>
    public Task<List<ClientDto>> GetTopSellersAsync(int topCount = 5);

    /// <summary>
    /// Получает статистику по количеству заявок по типам недвижимости
    /// </summary>
    public Task<List<(PropertyType Type, int Count)>> GetRequestsCountByPropertyTypeAsync();

    /// <summary>
    /// Получает клиентов с заявками минимальной стоимости
    /// </summary>
    public Task<List<ClientDto>> GetClientsWithMinAmountRequestsAsync();

    /// <summary>
    /// Получает клиентов, ищущих недвижимость заданного типа
    /// </summary>
    public Task<List<ClientDto>> GetClientsByPropertyTypeAsync(PropertyType propertyType);
}