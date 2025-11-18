using EstateAgency.Application.DTOs;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using EstateAgency.Domain.Interfaces;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с заявками на покупку и продажу недвижимости
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
public class RequestService(IRequestRepository repository) : IRequestService
{
    /// <summary>
    /// Получает список всех заявок
    /// </summary> 
    public async Task<List<RequestDto>> GetAllRequestsAsync()
    {
        var requests = await repository.GetAllAsync();
        return [.. requests.Select(r => new RequestDto
        {
            Id = r.Id,
            ClientId = r.ClientId,
            PropertyId = r.PropertyId,
            Type = r.Type,
            Amount = r.Amount,
            CreatedDate = r.CreatedDate,
            ClientFullName = r.Client?.FullName ?? string.Empty,
            PropertyAddress = r.Property?.Address ?? string.Empty
        })];
    }

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param> 
    public async Task<RequestDto?> GetRequestByIdAsync(int id)
    {
        var request = await repository.GetByIdAsync(id);
        if (request == null) return null;
        return new RequestDto
        {
            Id = request.Id,
            ClientId = request.ClientId,
            PropertyId = request.PropertyId,
            Type = request.Type,
            Amount = request.Amount,
            CreatedDate = request.CreatedDate,
            ClientFullName = request.Client?.FullName ?? string.Empty,
            PropertyAddress = request.Property?.Address ?? string.Empty
        };
    }

    /// <summary>
    /// Создает новую заявку
    /// </summary>
    /// <param name="requestDto">DTO с данными для создания заявки</param> 
    public async Task<RequestDto> CreateRequestAsync(CreateRequestDto requestDto)
    { 
        var request = new Request
        {
            ClientId = requestDto.ClientId,
            PropertyId = requestDto.PropertyId,
            Type = requestDto.Type,
            Amount = requestDto.Amount,
            CreatedDate = DateTime.UtcNow,
        };

        var createdRequest = await repository.AddAsync(request);

        return new RequestDto
        {
            Id = createdRequest.Id,
            ClientId = createdRequest.ClientId,
            ClientFullName = createdRequest.Client?.FullName ?? string.Empty,
            PropertyId = createdRequest.PropertyId,
            PropertyAddress = createdRequest.Property?.Address ?? string.Empty,
            Type = createdRequest.Type,
            Amount = createdRequest.Amount,
            CreatedDate = createdRequest.CreatedDate
        };
    }

    /// <summary>
    /// Обновляет данные заявки
    /// </summary>
    /// <param name="id">Идентификатор заявки для обновления</param>
    /// <param name="requestDto">DTO с обновленными данными заявки</param> 
    public async Task<RequestDto?> UpdateRequestAsync(int id, CreateRequestDto requestDto)
    {
        var existingRequest = await repository.GetByIdAsync(id);
        if (existingRequest == null) return null;

        existingRequest.ClientId = requestDto.ClientId;
        existingRequest.PropertyId = requestDto.PropertyId;
        existingRequest.Type = requestDto.Type;
        existingRequest.Amount = requestDto.Amount;

        var updatedRequest = await repository.UpdateAsync(existingRequest);
        if (updatedRequest == null) return null;

        return new RequestDto
        {
            Id = updatedRequest.Id,
            ClientId = updatedRequest.ClientId,
            ClientFullName = updatedRequest.Client?.FullName ?? string.Empty,
            PropertyId = updatedRequest.PropertyId,
            PropertyAddress = updatedRequest.Property?.Address ?? string.Empty,
            Type = updatedRequest.Type,
            Amount = updatedRequest.Amount,
            CreatedDate = updatedRequest.CreatedDate
        };
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
        var sellers = await repository.GetSellersByPeriodAsync(startDate, endDate);
        return [.. sellers.Select(s => new ClientDto
        {
            Id = s.Id,
            FullName = s.FullName,
            PassportNumber = s.PassportNumber,
            PhoneNumber = s.PhoneNumber
        })];
    }

    /// <summary>
    /// Получает список топ-N покупателей по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество покупателей для возврата</param>
    public async Task<List<ClientDto>> GetTopBuyersAsync(int topCount = 5)
    {
        var buyers = await repository.GetTopBuyersAsync(topCount);
        return [.. buyers.Select(b => new ClientDto
        {
            Id = b.Id,
            FullName = b.FullName,
            PassportNumber = b.PassportNumber,
            PhoneNumber = b.PhoneNumber
        })];
    }

    /// <summary>
    /// Получает список топ-N продавцов по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество продавцов для возврата</param>
    public async Task<List<ClientDto>> GetTopSellersAsync(int topCount = 5)
    {
        var sellers = await repository.GetTopSellersAsync(topCount);
        return [.. sellers.Select(s => new ClientDto
        {
            Id = s.Id,
            FullName = s.FullName,
            PassportNumber = s.PassportNumber,
            PhoneNumber = s.PhoneNumber
        })];
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
        return [.. clients.Select(c => new ClientDto
        {
            Id = c.Id,
            FullName = c.FullName,
            PassportNumber = c.PassportNumber,
            PhoneNumber = c.PhoneNumber
        })];
    }

    /// <summary>
    /// Получает клиентов по типу недвижимости
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для фильтрации</param>
    public async Task<List<ClientDto>> GetClientsByPropertyTypeAsync(PropertyType propertyType)
    {
        var clients = await repository.GetClientsByPropertyTypeAsync(propertyType);
        return [.. clients.Select(c => new ClientDto
        {
            Id = c.Id,
            FullName = c.FullName,
            PassportNumber = c.PassportNumber,
            PhoneNumber = c.PhoneNumber
        })];
    }
}