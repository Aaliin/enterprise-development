using EstateAgency.Application.DTOs;

namespace EstateAgency.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с клиентами 
/// </summary>
public interface IClientService
{
    /// <summary>
    /// Получает список всех клиентов
    /// </summary> 
    public Task<List<ClientDto>> GetAllClientsAsync();

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param> 
    public Task<ClientDto?> GetClientByIdAsync(int id);

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="clientDto">DTO для создания клиента</param> 
    public Task<ClientDto> CreateClientAsync(CreateClientDto clientDto);

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="clientDto">DTO с обновленными данными</param> 
    public Task<ClientDto?> UpdateClientAsync(int id, CreateClientDto clientDto);

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param> 
    public Task<bool> DeleteClientAsync(int id);
}