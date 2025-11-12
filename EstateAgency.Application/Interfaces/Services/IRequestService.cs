using EstateAgency.Application.DTOs;
using EstateAgency.Application.DTOs.Requests;

namespace EstateAgency.Application.Interfaces.Services;

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
    public Task<RequestDto> CreateRequestAsync(RequestCreateDto requestDto);

    /// <summary>
    /// Обновляет данные заявки
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="requestDto">DTO с обновленными данными</param>
    public Task<RequestDto?> UpdateRequestAsync(int id, RequestUpdateDto requestDto);

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    public Task<bool> DeleteRequestAsync(int id);
}