using AutoMapper;
using EstateAgency.Domain.Data;
﻿using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using EstateAgency.Domain.Interfaces;

namespace EstateAgency.Infrastructure.Repositories;

/// <summary>
/// Реализация репозитория заявок в памяти
/// </summary>
public class InMemoryRequestRepository : IRequestRepository
{
    private readonly List<Request> _requests = [];
    private readonly IClientRepository _clientRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    private int _nextId = 1;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория в памяти
    /// </summary>
    /// <param name="clientRepository">Репозиторий клиентов</param>
    /// <param name="propertyRepository">Репозиторий объектов недвижимости</param>
    public InMemoryRequestRepository(IClientRepository clientRepository, IPropertyRepository propertyRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _propertyRepository = propertyRepository;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        InitializeSampleDataAsync().Wait();
    }

    /// <summary>
    /// Инициализирует демонстрационные данные
    /// </summary>
    private async Task InitializeSampleDataAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        var properties = await _propertyRepository.GetAllAsync();

        if (clients.Count != 0 && properties.Count != 0)
        {
            var sampleRequests = SampleData.CreateSampleRequests(clients, properties);
            _requests.AddRange(sampleRequests);
            _nextId = sampleRequests.Count + 1;
        }
    }

    /// <summary>
    /// Получает все заявки
    /// </summary>
    public Task<List<Request>> GetAllAsync() => Task.FromResult(_requests.ToList());
    
    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<Request?> GetByIdAsync(int id) => Task.FromResult(_requests.FirstOrDefault(r => r.Id == id));
    
    /// <summary>
    /// Добавляет новую заявку
    /// </summary>
    /// <param name="request">Заявка для добавления</param>
    public async Task<Request> AddAsync(Request request)
    {
        var client = await _clientRepository.GetByIdAsync(request.ClientId);
        var property = await _propertyRepository.GetByIdAsync(request.PropertyId);

        if (client == null) throw new ArgumentException("Client not found");
        if (property == null) throw new ArgumentException("Property not found");

        request.Id = _nextId++;
        request.Client = client;
        request.Property = property;
        _requests.Add(request);

        return request;
    }

    /// <summary>
    /// Обновляет существующую заявку
    /// </summary>
    /// <param name="request">Заявка с обновленными данными</param>
    public async Task<Request?> UpdateAsync(Request request)
    {
        var existing = _requests.FirstOrDefault(r => r.Id == request.Id);
        if (existing == null) return null;

        var client = await _clientRepository.GetByIdAsync(request.ClientId);
        var property = await _propertyRepository.GetByIdAsync(request.PropertyId);

        if (client == null) throw new ArgumentException("Client not found");
        if (property == null) throw new ArgumentException("Property not found");

        _mapper.Map(request, existing);
        existing.Client = client;
        existing.Property = property;

        return existing;
    }

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param>
    public Task<bool> DeleteAsync(int id)
    {
        var request = _requests.FirstOrDefault(r => r.Id == id);
        if (request == null) return Task.FromResult(false);

        _requests.Remove(request);
        return Task.FromResult(true);
    }

    /// <summary>
    /// Проверяет существование заявки по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<bool> ExistsAsync(int id) => Task.FromResult(_requests.Any(r => r.Id == id));

    /// <summary>
    /// Получает продавцов за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param>
    public Task<List<Client>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var sellers = _requests
        .Where(r => r.Type == RequestType.Sale &&
                   r.CreatedDate >= startDate &&
                   r.CreatedDate <= endDate &&
                   r.Client != null)
            .Select(r => r.Client!)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        return Task.FromResult(sellers);
    }

    /// <summary>
    /// Получает топ-N покупателей по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество покупателей для возврата</param>
    public Task<List<Client>> GetTopBuyersAsync(int topCount = 5)
    {
        var topBuyers = _requests
            .Where(r => r.Type == RequestType.Purchase && r.Client != null)
            .GroupBy(r => r.Client!)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(topCount)
            .Select(x => x.Client)
            .ToList();

        return Task.FromResult(topBuyers);
    }

    /// <summary>
    /// Получает топ-N продавцов по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество продавцов для возврата</param>
    public Task<List<Client>> GetTopSellersAsync(int topCount = 5)
    {
        var topSellers = _requests
            .Where(r => r.Type == RequestType.Sale && r.Client != null)
            .GroupBy(r => r.Client!)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(topCount)
            .Select(x => x.Client)
            .ToList();

        return Task.FromResult(topSellers);
    }

    /// <summary>
    /// Получает количество заявок по типам недвижимости
    /// </summary>
    public Task<List<(PropertyType Type, int Count)>> GetRequestsCountByPropertyTypeAsync()
    {
        var counts = _requests
            .Where(r => r.Property != null) 
            .GroupBy(r => r.Property!.Type)
            .Select(g => (Type: g.Key, Count: g.Count()))
            .ToList();

        return Task.FromResult(counts);
    }

    /// <summary>
    /// Получает клиентов с заявками минимальной стоимости
    /// </summary>
    public Task<List<Client>> GetClientsWithMinAmountRequestsAsync()
    {
        var minPurchase = _requests.Where(r => r.Type == RequestType.Purchase).Min(r => r.Amount);
        var minSale = _requests.Where(r => r.Type == RequestType.Sale).Min(r => r.Amount);

        var clients = _requests
            .Where(r => r.Client != null && 
                       (r.Type == RequestType.Purchase && r.Amount == minPurchase) ||
                       (r.Type == RequestType.Sale && r.Amount == minSale))
            .Select(r => r.Client!)
            .Distinct()
            .ToList();

        return Task.FromResult(clients);
    }

    /// <summary>
    /// Получает клиентов по типу недвижимости
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для фильтрации</param>
    public Task<List<Client>> GetClientsByPropertyTypeAsync(PropertyType propertyType)
    {
        var clients = _requests
            .Where(r => r.Type == RequestType.Purchase &&
                       r.Property != null &&
                       r.Client != null &&
                       r.Property.Type == propertyType)
            .Select(r => r.Client!)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        return Task.FromResult(clients);
    }
}