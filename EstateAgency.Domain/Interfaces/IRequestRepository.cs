using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;

namespace EstateAgency.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с заявками
/// </summary>
public interface IRequestRepository
{
    /// <summary>
    /// Получает все заявки
    /// </summary>
    public Task<List<Request>> GetAllAsync();

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<Request?> GetByIdAsync(int id);

    /// <summary>
    /// Добавляет новую заявку
    /// </summary>
    /// <param name="request">Заявка для добавления</param>
    public Task<Request> AddAsync(Request request);

    /// <summary>
    /// Обновляет существующую заявку
    /// </summary>
    /// <param name="request">Заявка с обновленными данными</param>
    public Task<Request?> UpdateAsync(Request request);

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param>
    public Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Проверяет существование заявки по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Получает продавцов за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param>
    public Task<List<Client>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получает топ-N покупателей по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество покупателей для возврата</param>
    public Task<List<Client>> GetTopBuyersAsync(int topCount = 5);

    /// <summary>
    /// Получает топ-N продавцов по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество продавцов для возврата</param>
    public Task<List<Client>> GetTopSellersAsync(int topCount = 5);

    /// <summary>
    /// Получает количество заявок по типам недвижимости
    /// </summary>
    public Task<List<(PropertyType Type, int Count)>> GetRequestsCountByPropertyTypeAsync();

    /// <summary>
    /// Получает клиентов с заявками минимальной стоимости
    /// </summary>
    public Task<List<Client>> GetClientsWithMinAmountRequestsAsync();

    /// <summary>
    /// Получает клиентов по типу недвижимости
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для фильтрации</param>
    public Task<List<Client>> GetClientsByPropertyTypeAsync(PropertyType propertyType);
}