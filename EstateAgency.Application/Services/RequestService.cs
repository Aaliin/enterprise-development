using AutoMapper;
using EstateAgency.Application.Dto;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using EstateAgency.Domain.Interfaces;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с заявками на покупку и продажу недвижимости
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
/// <param name="mapper">AutoMapper для преобразования объектов</param>
public class RequestService(IRequestRepository repository, IMapper mapper) : IRequestService
{
    /// <summary>
    /// Получает список всех заявок
    /// </summary> 
    public async Task<List<RequestDto>> GetAllRequestsAsync()
    {
        var requests = await repository.GetAllAsync();
        return mapper.Map<List<RequestDto>>(requests);
    }

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param> 
    public async Task<RequestDto?> GetRequestByIdAsync(int id)
    {
        var request = await repository.GetByIdAsync(id);
        return mapper.Map<RequestDto?>(request);
    }

    /// <summary>
    /// Создает новую заявку
    /// </summary>
    /// <param name="requestDto">DTO с данными для создания заявки</param> 
    public async Task<RequestDto> CreateRequestAsync(CreateRequestDto requestDto)
    {
        ArgumentNullException.ThrowIfNull(requestDto);

        var request = mapper.Map<Request>(requestDto);
        request.CreatedDate = DateTime.UtcNow;

        var createdRequest = await repository.AddAsync(request);
        var requestWithDetails = await repository.GetByIdAsync(createdRequest.Id);
        return mapper.Map<RequestDto>(requestWithDetails);
    }

    /// <summary>
    /// Обновляет данные заявки
    /// </summary>
    /// <param name="id">Идентификатор заявки для обновления</param>
    /// <param name="requestDto">DTO с обновленными данными заявки</param> 
    public async Task<RequestDto?> UpdateRequestAsync(int id, CreateRequestDto requestDto)
    {
        ArgumentNullException.ThrowIfNull(requestDto);

        var existingRequest = await repository.GetByIdAsync(id);
        if (existingRequest == null) return null;

        mapper.Map(requestDto, existingRequest);
        var updatedRequest = await repository.UpdateAsync(existingRequest);

        if (updatedRequest == null) return null;

        var requestWithDetails = await repository.GetByIdAsync(updatedRequest.Id);
        return mapper.Map<RequestDto>(requestWithDetails);
    }

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param> 
    public async Task<bool> DeleteRequestAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    /// <summary>
    /// Получает список продавцов за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param>
    public async Task<List<ClientDto>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must be before or equal to end date");

        var sellers = await repository.GetSellersByPeriodAsync(startDate, endDate);
        return mapper.Map<List<ClientDto>>(sellers);
    }

    /// <summary>
    /// Получает список топ-N покупателей по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество покупателей для возврата</param>
    public async Task<List<ClientDto>> GetTopBuyersAsync(int topCount = 5)
    {
        var buyers = await repository.GetTopBuyersAsync(topCount);
        return mapper.Map<List<ClientDto>>(buyers);
    }

    /// <summary>
    /// Получает список топ-N продавцов по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество продавцов для возврата</param>
    public async Task<List<ClientDto>> GetTopSellersAsync(int topCount = 5)
    {
        if (topCount <= 0)
            throw new ArgumentException("Top count must be greater than 0", nameof(topCount));

        var sellers = await repository.GetTopSellersAsync(topCount);
        return mapper.Map<List<ClientDto>>(sellers);
    }

    /// <summary>
    /// Получает количество заявок по типам недвижимости
    /// </summary>
    public async Task<List<(PropertyType Type, int Count)>> GetRequestsCountByPropertyTypeAsync()
    {
        return await repository.GetRequestsCountByPropertyTypeAsync();
    }

    /// <summary>
    /// Получает клиентов с заявками минимальной стоимости
    /// </summary>
    public async Task<List<ClientDto>> GetClientsWithMinAmountRequestsAsync()
    {
        var clients = await repository.GetClientsWithMinAmountRequestsAsync();
        return mapper.Map<List<ClientDto>>(clients);
    }

    /// <summary>
    /// Получает клиентов по типу недвижимости
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для фильтрации</param>
    public async Task<List<ClientDto>> GetClientsByPropertyTypeAsync(PropertyType propertyType)
    {
        var clients = await repository.GetClientsByPropertyTypeAsync(propertyType);
        return mapper.Map<List<ClientDto>>(clients);
    }
}