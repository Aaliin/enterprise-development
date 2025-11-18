using EstateAgency.Domain.Entities;

namespace EstateAgency.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с объектами недвижимости
/// </summary>
public interface IPropertyRepository
{
    /// <summary>
    /// Получает все объекты недвижимости
    /// </summary>
    public Task<List<Property>> GetAllAsync();

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    public Task<Property?> GetByIdAsync(int id);

    /// <summary>
    /// Добавляет новый объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости для добавления</param>
    public Task<Property> AddAsync(Property property);

    /// <summary>
    /// Обновляет существующий объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости с обновленными данными</param>
    public Task<Property?> UpdateAsync(Property property);

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param>
    public Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Проверяет существование объекта недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    public Task<bool> ExistsAsync(int id);
}