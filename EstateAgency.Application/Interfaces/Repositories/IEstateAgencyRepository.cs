using EstateAgency.Domain;
using EstateAgency.Domain.Enum;

namespace EstateAgency.Application.Interfaces.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с данными риэлторского агентства 
/// </summary>
public interface IEstateAgencyRepository
{
    // Property CRUD

    /// <summary>
    /// Получает все объекты недвижимости
    /// </summary>
    public IEnumerable<Property> GetAllProperties();

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    public Property? GetPropertyById(int id);

    /// <summary>
    /// Добавляет новый объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости для добавления</param>
    public void AddProperty(Property property);

    /// <summary>
    /// Обновляет существующий объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости с обновленными данными</param>
    public void UpdateProperty(Property property);

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param>
    public void DeleteProperty(int id);

    // Client CRUD

    /// <summary>
    /// Получает всех клиентов
    /// </summary>
    public Task<List<Client>> GetClientsAsync();

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    public Task<Client?> GetClientByIdAsync(int id);

    /// <summary>
    /// Добавляет нового клиента
    /// </summary>
    /// <param name="client">Клиент для добавления</param>
    public Task<Client> AddClientAsync(Client client);

    /// <summary>
    /// Обновляет существующего клиента
    /// </summary>
    /// <param name="client">Клиент с обновленными данными</param>
    public Task<Client?> UpdateClientAsync(Client client);

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param>
    public Task<bool> DeleteClientAsync(int id);

    // Request CRUD

    /// <summary>
    /// Получает все заявки
    /// </summary>
    public Task<List<Request>> GetRequestsAsync();

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<Request?> GetRequestByIdAsync(int id);

    /// <summary>
    /// Добавляет новую заявку
    /// </summary>
    /// <param name="request">Заявка для добавления</param>
    public Task<Request> AddRequestAsync(Request request);

    /// <summary>
    /// Обновляет существующую заявку
    /// </summary>
    /// <param name="request">Заявка с обновленными данными</param>
    public Task<Request?> UpdateRequestAsync(Request request);

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param>
    public Task<bool> DeleteRequestAsync(int id);

    // Analytical queries

    /// <summary>
    /// Получает всех продавцов, оставивших заявки за заданный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param>
    public Task<List<Client>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получает топ-5 клиентов по количеству заявок указанного типа
    /// </summary>
    /// <param name="type">Тип заявки</param>
    public Task<List<(Client Client, int Count)>> GetTop5ClientsByRequests(RequestType type);

    /// <summary>
    /// Получает количество заявок по каждому типу недвижимости
    /// </summary>
    public Task<Dictionary<Domain.Enum.PropertyType, int>> GetRequestsByPropertyTypeAsync();

    /// <summary>
    /// Получает клиентов, открывших заявки с минимальной стоимостью
    /// </summary>
    public Task<List<Client>> GetClientsWithMinAmountRequestsAsync();

    /// <summary>
    /// Получает клиентов, ищущих недвижимость заданного типа
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для поиска</param>
    public Task<List<Client>> GetClientsByPropertyTypeAsync(Domain.Enum.PropertyType propertyType);
}