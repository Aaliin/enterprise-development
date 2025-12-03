using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interfaces;
using EstateAgency.Domain.Data;

namespace EstateAgency.Infrastructure.Repositories;

/// <summary>
/// Реализация репозитория клиентов в памяти
/// </summary>
public class InMemoryClientRepository : IClientRepository
{
    private readonly List<Client> _clients = [];
    private int _nextId = 1;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория в памяти
    /// </summary>
    public InMemoryClientRepository()
    {
        var testClients = SampleData.GetSampleClients();
        _clients.AddRange(testClients);
        _nextId = testClients.Count + 1;
    }

    /// <summary>
    /// Получает всех клиентов
    /// </summary>
    public Task<List<Client>> GetAllAsync() => Task.FromResult(_clients.ToList());

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    public Task<Client?> GetByIdAsync(int id) => Task.FromResult(_clients.FirstOrDefault(c => c.Id == id));

    /// <summary>
    /// Добавляет нового клиента
    /// </summary>
    /// <param name="client">Клиент для добавления</param>
    public Task<Client> AddAsync(Client client)
    {
        if (_clients.Any(c => c.PassportNumber == client.PassportNumber))
        {
            throw new InvalidOperationException("Client with this passport number already exists");
        }
        client.Id = _nextId++;
        _clients.Add(client);
        return Task.FromResult(client);
    }

    /// <summary>
    /// Обновляет существующего клиента
    /// </summary>
    /// <param name="client">Клиент с обновленными данными</param>
    public Task<Client?> UpdateAsync(Client client)
    {
        var existing = _clients.FirstOrDefault(c => c.Id == client.Id);
        if (existing == null) return Task.FromResult<Client?>(null);

        if (_clients.Any(c => c.Id != client.Id && c.PassportNumber == client.PassportNumber))
        {
            throw new InvalidOperationException("Another client with this passport number already exists");
        }
        existing.FullName = client.FullName;
        existing.PassportNumber = client.PassportNumber;
        existing.PhoneNumber = client.PhoneNumber;

        return Task.FromResult<Client?>(existing);
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param>
    public Task<bool> DeleteAsync(int id)
    {
        var client = _clients.FirstOrDefault(c => c.Id == id);
        if (client == null) return Task.FromResult(false);

        _clients.Remove(client);
        return Task.FromResult(true);
    }

    /// <summary>
    /// Проверяет существование клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    public Task<bool> ExistsAsync(int id) => Task.FromResult(_clients.Any(c => c.Id == id));
}