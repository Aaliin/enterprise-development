using EstateAgency.Domain.Entities;

namespace EstateAgency.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с клиентами
/// </summary>
public interface IClientRepository
{
    /// <summary>
    /// Получает всех клиентов
    /// </summary>
    public Task<List<Client>> GetAllAsync();

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    public Task<Client?> GetByIdAsync(int id);

    /// <summary>
    /// Добавляет нового клиента
    /// </summary>
    /// <param name="client">Клиент для добавления</param>
    public Task<Client> AddAsync(Client client);

    /// <summary>
    /// Обновляет существующего клиента
    /// </summary>
    /// <param name="client">Клиент с обновленными данными</param>
    public Task<Client?> UpdateAsync(Client client);

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param>
    public Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Проверяет существование клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    public Task<bool> ExistsAsync(int id);
}