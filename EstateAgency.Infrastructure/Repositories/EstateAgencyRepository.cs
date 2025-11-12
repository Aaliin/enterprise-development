using EstateAgency.Application.Interfaces.Repositories;
using EstateAgency.Domain;
using EstateAgency.Domain.Enum;
using EstateAgency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructure.Repositories;

/// <summary>
/// Обеспечивает выполнение CRUD операций и аналитических запросов
/// </summary>
public class EstateAgencyRepository(ApplicationDbContext context) : IEstateAgencyRepository
{
    private readonly ApplicationDbContext _context = context;

    // Property CRUD 

    /// <summary>
    /// Получает все объекты недвижимости
    /// </summary> 
    public IEnumerable<Property> GetAllProperties()
    {
        return [.. _context.Properties];
    }

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param> 
    public Property? GetPropertyById(int id)
    {
        return _context.Properties.Find(id);
    }

    /// <summary>
    /// Добавляет новый объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости для добавления</param>
    public void AddProperty(Property property)
    {
        _context.Properties.Add(property);
        _context.SaveChanges();
    }

    /// <summary>
    /// Обновляет существующий объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости с обновленными данными</param>
    public void UpdateProperty(Property property)
    {
        _context.Properties.Update(property);
        _context.SaveChanges();
    }

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param>
    public void DeleteProperty(int id)
    {
        var property = _context.Properties.Find(id);
        if (property != null)
        {
            _context.Properties.Remove(property);
            _context.SaveChanges();
        }
    }

    // Client CRUD  

    /// <summary>
    /// Получает всех клиентов
    /// </summary> 
    public async Task<List<Client>> GetClientsAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param> 
    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    /// <summary>
    /// Добавляет нового клиента
    /// </summary>
    /// <param name="client">Клиент для добавления</param> 
    public async Task<Client> AddClientAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    /// <summary>
    /// Обновляет существующего клиента
    /// </summary>
    /// <param name="client">Клиент с обновленными данными</param> 
    public async Task<Client?> UpdateClientAsync(Client client)
    {
        var existingClient = await _context.Clients.FindAsync(client.Id);
        if (existingClient == null)
            return null;

        _context.Entry(existingClient).CurrentValues.SetValues(client);
        await _context.SaveChangesAsync();
        return existingClient;
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param> 
    public async Task<bool> DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
            return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }

    // Request CRUD  

    /// <summary>
    /// Получает все заявки с включением связанных данных о клиентах и объектах недвижимости
    /// </summary> 
    public async Task<List<Request>> GetRequestsAsync()
    {
        return await _context.Requests
            .Include(r => r.Client)
            .Include(r => r.Property)
            .ToListAsync();
    }

    /// <summary>
    /// Получает заявку по идентификатору с включением связанных данных
    /// </summary>
    /// <param name="id">Идентификатор заявки</param> 
    public async Task<Request?> GetRequestByIdAsync(int id)
    {
        return await _context.Requests
            .Include(r => r.Client)
            .Include(r => r.Property)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Добавляет новую заявку
    /// </summary>
    /// <param name="request">Заявка для добавления</param> 
    public async Task<Request> AddRequestAsync(Request request)
    {
        _context.Requests.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    /// <summary>
    /// Обновляет существующую заявку
    /// </summary>
    /// <param name="request">Заявка с обновленными данными</param> 
    public async Task<Request?> UpdateRequestAsync(Request request)
    {
        var existingRequest = await _context.Requests.FindAsync(request.Id);
        if (existingRequest == null)
            return null;

        _context.Entry(existingRequest).CurrentValues.SetValues(request);
        await _context.SaveChangesAsync();
        return existingRequest;
    }

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param> 
    public async Task<bool> DeleteRequestAsync(int id)
    {
        var request = await _context.Requests.FindAsync(id);
        if (request == null)
            return false;

        _context.Requests.Remove(request);
        await _context.SaveChangesAsync();
        return true;
    }

    // Analytical 

    /// <summary>
    /// Получает всех продавцов, оставивших заявки за заданный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param> 
    public async Task<List<Client>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Requests
            .Where(r => r.Type == RequestType.Sale &&
                       r.CreatedDate >= startDate &&
                       r.CreatedDate <= endDate)
            .Select(r => r.Client)
            .Distinct()
            .ToListAsync();
    }

    /// <summary>
    /// Получает топ-5 клиентов по количеству заявок указанного типа
    /// </summary>
    /// <param name="type">Тип заявки (покупка/продажа)</param> 
    public async Task<List<(Client Client, int Count)>> GetTop5ClientsByRequests(RequestType type)
    {
        var results = await _context.Requests
            .Where(r => r.Type == type)
            .GroupBy(r => r.Client)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync();

        return [.. results.Select(x => (x.Client, x.Count))];
    }

    /// <summary>
    /// Получает количество заявок по каждому типу недвижимости
    /// </summary> 
    public async Task<Dictionary<PropertyType, int>> GetRequestsByPropertyTypeAsync()
    {
        return await _context.Requests
            .Include(r => r.Property)
            .GroupBy(r => r.Property.Type)
            .Select(g => new { PropertyType = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.PropertyType, x => x.Count);
    }

    /// <summary>
    /// Получает клиентов, открывших заявки с минимальной стоимостью
    /// </summary> 
    public async Task<List<Client>> GetClientsWithMinAmountRequestsAsync()
    {
        var minAmount = await _context.Requests.MinAsync(r => r.Amount);
        return await _context.Requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Client)
            .Distinct()
            .ToListAsync();
    }

    /// <summary>
    /// Получает клиентов, ищущих недвижимость заданного типа
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для поиска</param> 
    public async Task<List<Client>> GetClientsByPropertyTypeAsync(PropertyType propertyType)
    {
        return await _context.Requests
            .Include(r => r.Property)
            .Where(r => r.Property.Type == propertyType)
            .Select(r => r.Client)
            .Distinct()
            .ToListAsync();
    }
}