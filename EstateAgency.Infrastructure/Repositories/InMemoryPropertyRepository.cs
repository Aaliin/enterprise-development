using AutoMapper;
using EstateAgency.Domain.Data;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using EstateAgency.Domain.Interfaces;

namespace EstateAgency.Infrastructure.Repositories;

/// <summary>
/// Реализация репозитория объектов недвижимости в памяти
/// </summary>
public class InMemoryPropertyRepository : IPropertyRepository
{
    private readonly List<Property> _properties = [];
    private readonly IMapper _mapper;
    private int _nextId = 1;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория в памяти
    /// </summary>
    public InMemoryPropertyRepository(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        var testProperties = SampleData.GetSampleProperties();
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
    /// Находит объект недвижимости по кадастровому номеру
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер</param>
    public Task<Property?> GetByCadastralNumberAsync(string cadastralNumber)
    {
        var property = _properties.FirstOrDefault(p =>
            string.Equals(p.CadastralNumber, cadastralNumber, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(property);
    }

    /// <summary>
    /// Получает список объектов недвижимости по типу
    /// </summary>
    /// <param name="type">Тип недвижимости</param>
    public Task<List<Property>> GetByTypeAsync(PropertyType type)
    {
        var properties = _properties
            .Where(p => p.Type == type)
            .ToList();
        return Task.FromResult(properties);
    }

    /// <summary>
    /// Добавляет новый объект недвижимости
    /// </summary>
    /// <param name="property">Объект недвижимости для добавления</param>
    public Task<Property> AddAsync(Property property)
    {
        if (_properties.Any(p => p.CadastralNumber == property.CadastralNumber))
        {
            throw new InvalidOperationException("Property with this cadastral number already exists");
        }
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

        if (_properties.Any(p => p.Id != property.Id && p.CadastralNumber == property.CadastralNumber))
        {
            throw new InvalidOperationException("Another property with this cadastral number already exists");
        }

        _mapper.Map(property, existing);
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

    /// <summary>
    /// Проверяет существование объекта недвижимости с указанным кадастровым номером
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер для проверки</param>
    public Task<bool> ExistsByCadastralNumberAsync(string cadastralNumber)
    {
        var exists = _properties.Any(p =>
            string.Equals(p.CadastralNumber, cadastralNumber, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(exists);
    }
}