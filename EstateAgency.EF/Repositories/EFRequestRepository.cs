using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using EstateAgency.Domain.Interfaces;
using EstateAgency.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.EF.Repositories;

/// <summary>
/// Репозиторий для работы с заявками на операции с недвижимостью с использованием Entity Framework
/// </summary>
public class EfRequestRepository(EstateAgencyDbContext context) : IRequestRepository
{
    /// <summary>
    /// Получает список всех заявок с включенными данными о клиентах и объектах недвижимости
    /// </summary>
    /// <returns>Список всех заявок с связанными данными</returns>
    public async Task<List<Request>> GetAllAsync()
    {
        return await context.Requests
            .Include(r => r.Client)
            .Include(r => r.Property)
            .ToListAsync();
    }

    /// <summary>
    /// Находит заявку по указанному идентификатору 
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <returns>Найденная заявка или null, если заявка не существует</returns>
    public async Task<Request?> GetByIdAsync(int id)
    {
        return await context.Requests
            .Include(r => r.Client)
            .Include(r => r.Property)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Добавляет новую заявку в базу данных
    /// </summary>
    /// <param name="request">Объект заявки для добавления</param>
    /// <returns>Добавленная заявка с присвоенным идентификатором</returns>
    public async Task<Request> AddAsync(Request request)
    {
        context.Requests.Add(request);
        await context.SaveChangesAsync();
        return request;
    }

    /// <summary>
    /// Обновляет данные существующей заявки
    /// </summary>
    /// <param name="request">Объект заявки с обновленными данными</param>
    /// <returns>Обновленная заявка или null, если заявка не найдена</returns>
    public async Task<Request?> UpdateAsync(Request request)
    {
        var existingRequest = await context.Requests.FindAsync(request.Id);
        if (existingRequest == null)
            return null;

        context.Entry(existingRequest).CurrentValues.SetValues(request);
        await context.SaveChangesAsync();
        return existingRequest;
    }

    /// <summary>
    /// Удаляет заявку по указанному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param>
    /// <returns>true, если заявка была удалена; false, если заявка не найдена</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var request = await context.Requests.FindAsync(id);
        if (request == null)
            return false;

        context.Requests.Remove(request);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Проверяет существование заявки с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор заявки для проверки</param>
    /// <returns>true, если заявка существует; false, если заявка не найдена</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await context.Requests.AnyAsync(r => r.Id == id);
    }

    /// <summary>
    /// Получает список продавцов, которые подавали заявки на продажу в указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param>
    /// <returns>Список клиентов-продавцов за указанный период</returns>
    public async Task<List<Client>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate) => await context.Requests
        .Include(r => r.Client)
        .Where(r => r.Type == RequestType.Sale &&
                   r.CreatedDate >= startDate &&
                   r.CreatedDate <= endDate &&
                   r.Client != null)
        .Select(r => r.Client!)
        .Distinct()
        .OrderBy(c => c.FullName)
        .ToListAsync();

    /// <summary>
    /// Получает список топ-N покупателей по количеству заявок на покупку
    /// </summary>
    /// <param name="topCount">Количество возвращаемых топ-покупателей (по умолчанию 5)</param>
    /// <returns>Список клиентов-покупателей с наибольшим количеством заявок на покупку</returns>
    public async Task<List<Client>> GetTopBuyersAsync(int topCount = 5)
        => await context.Requests
            .Include(r => r.Client)
            .Where(r => r.Type == RequestType.Purchase && r.Client != null)
            .GroupBy(r => r.Client!)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(topCount)
            .Select(x => x.Client)
            .ToListAsync();

    /// <summary>
    /// Получает список топ-N продавцов по количеству заявок на продажу
    /// </summary>
    /// <param name="topCount">Количество возвращаемых топ-продавцов (по умолчанию 5)</param>
    /// <returns>Список клиентов-продавцов с наибольшим количеством заявок на продажу</returns>
    public async Task<List<Client>> GetTopSellersAsync(int topCount = 5)
        => await context.Requests
            .Include(r => r.Client)
            .Where(r => r.Type == RequestType.Sale && r.Client != null)
            .GroupBy(r => r.Client!)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(topCount)
            .Select(x => x.Client)
            .ToListAsync();

    /// <summary>
    /// Получает статистику количества заявок по типам недвижимости
    /// </summary>
    /// <returns>Список кортежей (тип недвижимости, количество заявок)</returns>
    public async Task<List<(PropertyType Type, int Count)>> GetRequestsCountByPropertyTypeAsync()
    {
        var results = await context.Requests
            .Include(r => r.Property)
            .Where(r => r.Property != null)
            .GroupBy(r => r.Property!.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToListAsync();

        return [.. results.Select(x => (x.Type, x.Count))];
    }

    /// <summary>
    /// Получает список клиентов с заявками на минимальную сумму
    /// </summary>
    /// <returns>Список клиентов, у которых есть заявки с минимальной суммой</returns>
    public async Task<List<Client>> GetClientsWithMinAmountRequestsAsync()
    {
        if (!await context.Requests.AnyAsync())
        {
            return [];
        }

        var minAmount = await context.Requests
            .MinAsync(r => r.Amount);

        return await context.Requests
            .Include(r => r.Client)
            .Where(r => r.Amount == minAmount && r.Client != null)
            .Select(r => r.Client!)
            .Distinct()
            .ToListAsync();
    }

    /// <summary>
    /// Получает список клиентов, подававших заявки на покупку определенного типа недвижимости
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для фильтрации</param>
    /// <returns>Список клиентов, отсортированный по полному имени</returns>
    public async Task<List<Client>> GetClientsByPropertyTypeAsync(PropertyType propertyType)
        => await context.Requests
            .Include(r => r.Client)
            .Include(r => r.Property)
            .Where(r => r.Type == RequestType.Purchase &&
                       r.Property != null &&
                       r.Client != null &&
                       r.Property.Type == propertyType)
            .Select(r => r.Client!)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToListAsync();
}