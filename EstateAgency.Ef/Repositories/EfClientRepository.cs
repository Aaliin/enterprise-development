using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interfaces;
using EstateAgency.Ef.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Ef.Repositories;

/// <summary>
/// Репозиторий для работы с клиентами агентства недвижимости с использованием Entity Framework
/// </summary>
public class EfClientRepository(EstateAgencyDbContext context) : IClientRepository
{
    /// <summary>
    /// Получает список всех клиентов из базы данных
    /// </summary>
    /// <returns>Список всех клиентов</returns>
    public async Task<List<Client>> GetAllAsync() => await context.Clients.ToListAsync();

    /// <summary>
    /// Находит клиента по указанному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Найденный клиент или null, если клиент не существует</returns>
    public async Task<Client?> GetByIdAsync(int id) => await context.Clients.FindAsync(id);

    /// <summary>
    /// Добавляет нового клиента в базу данных
    /// </summary>
    /// <param name="client">Объект клиента для добавления</param>
    /// <returns>Добавленный клиент с присвоенным идентификатором</returns>
    public async Task<Client> AddAsync(Client client)
    {
        context.Clients.Add(client);
        await context.SaveChangesAsync();
        return client;
    }

    /// <summary>
    /// Находит клиента по номеру паспорта
    /// </summary>
    /// <param name="passportNumber">Номер паспорта</param>
    public async Task<Client?> GetByPassportNumberAsync(string passportNumber)
    {
        return await context.Clients.FirstOrDefaultAsync(c => c.PassportNumber == passportNumber);
    }

    /// <summary>
    /// Обновляет данные существующего клиента
    /// </summary>
    /// <param name="client">Объект клиента с обновленными данными</param>
    /// <returns>Обновленный клиент или null, если клиент не найден</returns>
    public async Task<Client?> UpdateAsync(Client client)
    {
        var existingClient = await context.Clients.FindAsync(client.Id);
        if (existingClient == null)
            return null;

        context.Entry(existingClient).CurrentValues.SetValues(client);
        await context.SaveChangesAsync();
        return existingClient;
    }

    /// <summary>
    /// Удаляет клиента по указанному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param>
    /// <returns>true, если клиент был удален; false, если клиент не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var client = await context.Clients.FindAsync(id);
        if (client == null)
            return false;

        context.Clients.Remove(client);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Проверяет существование клиента с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор клиента для проверки</param>
    /// <returns>true, если клиент существует; false, если клиент не найден</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await context.Clients.AnyAsync(c => c.Id == id);
    }

    /// <summary>
    /// Проверяет существование клиента с указанным номером паспорта
    /// </summary>
    /// <param name="passportNumber">Номер паспорта для проверки</param>
    public async Task<bool> ExistsByPassportNumberAsync(string passportNumber)
    {
        return await context.Clients.AnyAsync(c => c.PassportNumber == passportNumber);
    }
}