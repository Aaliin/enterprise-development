using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interfaces;
using EstateAgency.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.EF.Repositories;

/// <summary>
/// Репозиторий для работы с объектами недвижимости с использованием Entity Framework
/// </summary>
public class EfPropertyRepository(EstateAgencyDbContext context) : IPropertyRepository
{
    /// <summary>
    /// Получает список всех объектов недвижимости из базы данных
    /// </summary>
    /// <returns>Список всех объектов недвижимости</returns>
    public async Task<List<Property>> GetAllAsync() => await context.Properties.ToListAsync();

    /// <summary>
    /// Находит объект недвижимости по указанному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    /// <returns>Найденный объект недвижимости или null, если объект не существует</returns>
    public async Task<Property?> GetByIdAsync(int id) => await context.Properties.FindAsync(id);

    /// <summary>
    /// Добавляет новый объект недвижимости в базу данных
    /// </summary>
    /// <param name="property">Объект недвижимости для добавления</param>
    /// <returns>Добавленный объект недвижимости с присвоенным идентификатором</returns>
    public async Task<Property> AddAsync(Property property)
    {
        context.Properties.Add(property);
        await context.SaveChangesAsync();
        return property;
    }

    /// <summary>
    /// Обновляет данные существующего объекта недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости с обновленными данными</param>
    /// <returns>Обновленный объект недвижимости или null, если объект не найден</returns>
    public async Task<Property?> UpdateAsync(Property property)
    {
        var existingProperty = await context.Properties.FindAsync(property.Id);
        if (existingProperty == null)
            return null;

        context.Entry(existingProperty).CurrentValues.SetValues(property);
        await context.SaveChangesAsync();
        return existingProperty;
    }

    /// <summary>
    /// Удаляет объект недвижимости по указанному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param>
    /// <returns>true, если объект был удален; false, если объект не найден</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var property = await context.Properties.FindAsync(id);
        if (property == null)
            return false;

        context.Properties.Remove(property);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Проверяет существование объекта недвижимости с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для проверки</param>
    /// <returns>true, если объект существует; false, если объект не найден</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await context.Properties.AnyAsync(p => p.Id == id);
    }
}