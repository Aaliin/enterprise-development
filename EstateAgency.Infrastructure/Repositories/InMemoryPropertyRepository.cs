using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interfaces;
using EstateAgency.Infrastructure.Data;

namespace EstateAgency.Infrastructure.Repositories;

/// <summary>
/// Реализация репозитория объектов недвижимости в памяти
/// </summary>
public class InMemoryPropertyRepository : IPropertyRepository
{
    private readonly List<Property> _properties = [];
    private int _nextId = 1;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория в памяти
    /// </summary>
    public InMemoryPropertyRepository()
    {
        var testProperties = DataSeeder.GetTestProperties();
        _properties.AddRange(testProperties);
        _nextId = testProperties.Count + 1;
    }

    /// <summary>
    /// Получает все объекты недвижимости
    /// </summary>
    public Task<List<Property>> GetAllAsync() => Task.FromResult(_properties.ToList());

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    public Task<Property?> GetByIdAsync(int id) => Task.FromResult(_properties.FirstOrDefault(p => p.Id == id));

    /// <summary>
    /// Добавляет новый объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости для добавления</param>
    public Task<Property> AddAsync(Property property)
    {
        property.Id = _nextId++;
        _properties.Add(property);
        return Task.FromResult(property);
    }

    /// <summary>
    /// Обновляет существующий объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости с обновленными данными</param>
    public Task<Property?> UpdateAsync(Property property)
    {
        var existing = _properties.FirstOrDefault(p => p.Id == property.Id);
        if (existing == null) return Task.FromResult<Property?>(null);

        existing.Type = property.Type;
        existing.Purpose = property.Purpose;
        existing.CadastralNumber = property.CadastralNumber;
        existing.Address = property.Address;
        existing.Floors = property.Floors;
        existing.TotalArea = property.TotalArea;
        existing.Rooms = property.Rooms;
        existing.CeilingHeight = property.CeilingHeight;
        existing.Floor = property.Floor;
        existing.HasEncumbrances = property.HasEncumbrances;

        return Task.FromResult<Property?>(existing);
    }

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param>
    public Task<bool> DeleteAsync(int id)
    {
        var property = _properties.FirstOrDefault(p => p.Id == id);
        if (property == null) return Task.FromResult(false);

        _properties.Remove(property);
        return Task.FromResult(true);
    }

    /// <summary>
    /// Проверяет существование объекта недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    public Task<bool> ExistsAsync(int id) => Task.FromResult(_properties.Any(p => p.Id == id));
}