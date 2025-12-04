using AutoMapper;
using EstateAgency.Application.Dto;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interfaces;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с клиентскими данными
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
/// <param name="mapper">AutoMapper для преобразования объектов</param>
public class ClientService(IClientRepository repository, IMapper mapper) : IClientService
{
    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    public async Task<List<ClientDto>> GetAllClientsAsync()
    {
        var clients = await repository.GetAllAsync();
        return mapper.Map<List<ClientDto>>(clients);
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param> 
    public async Task<ClientDto?> GetClientByIdAsync(int id)
    {
        var client = await repository.GetByIdAsync(id);
        return mapper.Map<ClientDto?>(client);
    }

    /// <summary>
    /// Получает клиента по номеру паспорта
    /// </summary>
    /// <param name="passportNumber">Номер паспорта</param>
    public async Task<ClientDto?> GetClientByPassportAsync(string passportNumber)
    {
        if (string.IsNullOrWhiteSpace(passportNumber))
            throw new ArgumentException("Passport number cannot be empty", nameof(passportNumber));

        var client = await repository.GetByPassportNumberAsync(passportNumber);
        return mapper.Map<ClientDto?>(client);
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="clientDto">DTO с данными для создания клиента</param> 
    public async Task<ClientDto> CreateClientAsync(CreateClientDto clientDto)
    {
        ArgumentNullException.ThrowIfNull(clientDto);

        if (!string.IsNullOrWhiteSpace(clientDto.PassportNumber))
        {
            var existingClient = await repository.GetByPassportNumberAsync(clientDto.PassportNumber);
            if (existingClient != null)
                throw new InvalidOperationException($"Client with passport number {clientDto.PassportNumber} already exists");
        }
        var client = mapper.Map<Client>(clientDto);
        var createdClient = await repository.AddAsync(client);
        return mapper.Map<ClientDto>(createdClient);
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента для обновления</param>
    /// <param name="clientDto">DTO с обновленными данными клиента</param> 
    public async Task<ClientDto?> UpdateClientAsync(int id, CreateClientDto clientDto)
    {
        ArgumentNullException.ThrowIfNull(clientDto);

        var existingClient = await repository.GetByIdAsync(id);
        if (existingClient == null)
            return null;

        if (!string.IsNullOrWhiteSpace(clientDto.PassportNumber) &&
            clientDto.PassportNumber != existingClient.PassportNumber)
        {
            var clientWithSamePassport = await repository.GetByPassportNumberAsync(clientDto.PassportNumber);
            if (clientWithSamePassport != null && clientWithSamePassport.Id != id)
                throw new InvalidOperationException($"Another client with passport number {clientDto.PassportNumber} already exists");
        }

        mapper.Map(clientDto, existingClient);
        var updatedClient = await repository.UpdateAsync(existingClient);

        return mapper.Map<ClientDto>(updatedClient);
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param> 
    public async Task<bool> DeleteClientAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    /// <summary>
    /// Проверяет существование клиента с указанным номером паспорта
    /// </summary>
    /// <param name="passportNumber">Номер паспорта для проверки</param>
    /// <returns>true, если клиент существует; false, если клиент не найден</returns>
    public async Task<bool> ClientExistsByPassportAsync(string passportNumber)
    {
        if (string.IsNullOrWhiteSpace(passportNumber))
            throw new ArgumentException("Passport number cannot be empty", nameof(passportNumber));

        return await repository.ExistsByPassportNumberAsync(passportNumber);
    }
}