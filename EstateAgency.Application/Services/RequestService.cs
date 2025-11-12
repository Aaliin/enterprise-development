using EstateAgency.Application.DTOs.Requests;
using EstateAgency.Application.Exceptions;
using EstateAgency.Application.Interfaces.Repositories;
using EstateAgency.Application.Interfaces.Services;
using EstateAgency.Domain;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с заявками на покупку и продажу недвижимости
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
public class RequestService(IEstateAgencyRepository repository) : IRequestService
{
    private readonly IEstateAgencyRepository _repository = repository;

    /// <summary>
    /// Получает список всех заявок
    /// </summary> 
    public async Task<List<RequestDto>> GetAllRequestsAsync()
    {
        var requests = await _repository.GetRequestsAsync();
        return [.. requests.Select(r => new RequestDto
        {
            Id = r.Id,
            ClientId = r.ClientId,
            PropertyId = r.PropertyId,
            Type = r.Type,
            Amount = r.Amount,
            CreatedDate = r.CreatedDate
        })];
    }

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param> 
    public async Task<RequestDto?> GetRequestByIdAsync(int id)
    {
        var request = await _repository.GetRequestByIdAsync(id);
        return request is null ? null : new RequestDto
        {
            Id = request.Id,
            ClientId = request.ClientId,
            PropertyId = request.PropertyId,
            Type = request.Type,
            Amount = request.Amount,
            CreatedDate = request.CreatedDate
        };
    }

    /// <summary>
    /// Создает новую заявку
    /// </summary>
    /// <param name="requestDto">DTO с данными для создания заявки</param> 
    public async Task<RequestDto> CreateRequestAsync(RequestCreateDto requestDto)
    { 
        var client = await _repository.GetClientByIdAsync(requestDto.ClientId)
            ?? throw new EntityNotFoundException("Client", requestDto.ClientId);

        var property = _repository.GetPropertyById(requestDto.PropertyId)
            ?? throw new EntityNotFoundException("Property", requestDto.PropertyId);

        var request = new Request
        {
            ClientId = requestDto.ClientId,
            PropertyId = requestDto.PropertyId,
            Type = requestDto.Type,
            Amount = requestDto.Amount,
            CreatedDate = DateTime.UtcNow,
            Client = client,
            Property = property
        };

        var createdRequest = await _repository.AddRequestAsync(request);

        return new RequestDto
        {
            Id = createdRequest.Id,
            ClientId = createdRequest.ClientId,
            PropertyId = createdRequest.PropertyId,
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
    public async Task<RequestDto?> UpdateRequestAsync(int id, RequestUpdateDto requestDto)
    {
        var existingRequest = await _repository.GetRequestByIdAsync(id)
            ?? throw new EntityNotFoundException("Request", id);
         
        if (requestDto.ClientId.HasValue)
        {
            var client = await _repository.GetClientByIdAsync(requestDto.ClientId.Value)
                ?? throw new EntityNotFoundException("Client", requestDto.ClientId.Value);
            existingRequest.ClientId = requestDto.ClientId.Value;
            existingRequest.Client = client;
        }
         
        if (requestDto.PropertyId.HasValue)
        {
            var property = _repository.GetPropertyById(requestDto.PropertyId.Value)
                ?? throw new EntityNotFoundException("Property", requestDto.PropertyId.Value);
            existingRequest.PropertyId = requestDto.PropertyId.Value;
            existingRequest.Property = property;
        }
         
        if (requestDto.Type.HasValue) existingRequest.Type = requestDto.Type.Value;
        if (requestDto.Amount.HasValue) existingRequest.Amount = requestDto.Amount.Value;

        var updatedRequest = await _repository.UpdateRequestAsync(existingRequest);

        return updatedRequest is null ? null : new RequestDto
        {
            Id = updatedRequest.Id,
            ClientId = updatedRequest.ClientId,
            PropertyId = updatedRequest.PropertyId,
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
        return await _repository.DeleteRequestAsync(id);
    }
}