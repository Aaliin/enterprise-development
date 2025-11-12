using EstateAgency.Application.DTOs.Clients;
using EstateAgency.Application.Exceptions;
using EstateAgency.Application.Interfaces.Repositories;
using EstateAgency.Application.Interfaces.Services;
using EstateAgency.Domain;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с клиентскими данными
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
public class ClientService(IEstateAgencyRepository repository) : IClientService
{
    private readonly IEstateAgencyRepository _repository = repository;

    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    public async Task<List<ClientDto>> GetAllClientsAsync()
    {
        var clients = await _repository.GetClientsAsync();
        return [.. clients.Select(c => new ClientDto
        {
            Id = c.Id,
            FullName = c.FullName,
            PassportNumber = c.PassportNumber,
            PhoneNumber = c.PhoneNumber
        })];
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param> 
    public async Task<ClientDto?> GetClientByIdAsync(int id)
    {
        var client = await _repository.GetClientByIdAsync(id);
        return client is null ? null : new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            PassportNumber = client.PassportNumber,
            PhoneNumber = client.PhoneNumber
        };
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="clientDto">DTO с данными для создания клиента</param> 
    public async Task<ClientDto> CreateClientAsync(ClientCreateDto clientDto)
    {
        var client = new Client
        {
            FullName = clientDto.FullName,
            PassportNumber = clientDto.PassportNumber,
            PhoneNumber = clientDto.PhoneNumber
        };

        var createdClient = await _repository.AddClientAsync(client);

        return new ClientDto
        {
            Id = createdClient.Id,
            FullName = createdClient.FullName,
            PassportNumber = createdClient.PassportNumber,
            PhoneNumber = createdClient.PhoneNumber
        };
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента для обновления</param>
    /// <param name="clientDto">DTO с обновленными данными клиента</param> 
    public async Task<ClientDto?> UpdateClientAsync(int id, ClientUpdateDto clientDto)
    {
        var existingClient = await _repository.GetClientByIdAsync(id)
            ?? throw new EntityNotFoundException("Client", id);

        existingClient.FullName = clientDto.FullName;
        existingClient.PassportNumber = clientDto.PassportNumber;
        existingClient.PhoneNumber = clientDto.PhoneNumber;

        var updatedClient = await _repository.UpdateClientAsync(existingClient);

        return updatedClient is null ? null : new ClientDto
        {
            Id = updatedClient.Id,
            FullName = updatedClient.FullName,
            PassportNumber = updatedClient.PassportNumber,
            PhoneNumber = updatedClient.PhoneNumber
        };
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param> 
    public async Task<bool> DeleteClientAsync(int id)
    {
        return await _repository.DeleteClientAsync(id);
    }
}