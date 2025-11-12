using EstateAgency.Application.DTOs.Properties;
using EstateAgency.Application.Exceptions;
using EstateAgency.Application.Interfaces.Repositories;
using EstateAgency.Application.Interfaces.Services;
using EstateAgency.Domain;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с каталогом недвижимости
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
public class PropertyService(IEstateAgencyRepository repository) : IPropertyService
{
    private readonly IEstateAgencyRepository _repository = repository;

    /// <summary>
    /// Получает список всех объектов недвижимости
    /// </summary>
    public async Task<List<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = _repository.GetAllProperties();
        return [.. properties.Select(p => new PropertyDto
        {
            Id = p.Id,
            Type = p.Type,
            Purpose = p.Purpose,
            CadastralNumber = p.CadastralNumber,
            Address = p.Address,
            Floors = p.Floors,
            TotalArea = p.TotalArea,
            Rooms = p.Rooms,
            CeilingHeight = p.CeilingHeight,
            Floor = p.Floor,
            HasEncumbrances = p.HasEncumbrances
        })];
    }

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
    {
        var property = _repository.GetPropertyById(id);
        return property is null ? null : new PropertyDto
        {
            Id = property.Id,
            Type = property.Type,
            Purpose = property.Purpose,
            CadastralNumber = property.CadastralNumber,
            Address = property.Address,
            Floors = property.Floors,
            TotalArea = property.TotalArea,
            Rooms = property.Rooms,
            CeilingHeight = property.CeilingHeight,
            Floor = property.Floor,
            HasEncumbrances = property.HasEncumbrances
        };
    }

    /// <summary>
    /// Создает новый объект недвижимости
    /// </summary>
    /// <param name="propertyDto">DTO с данными для создания объекта недвижимости</param> 
    public async Task<PropertyDto> CreatePropertyAsync(PropertyCreateDto propertyDto)
    {
        var property = new Property
        {
            Type = propertyDto.Type,
            Purpose = propertyDto.Purpose,
            CadastralNumber = propertyDto.CadastralNumber,
            Address = propertyDto.Address,
            Floors = propertyDto.Floors,
            TotalArea = propertyDto.TotalArea,
            Rooms = propertyDto.Rooms,
            CeilingHeight = propertyDto.CeilingHeight,
            Floor = propertyDto.Floor,
            HasEncumbrances = propertyDto.HasEncumbrances
        };

        _repository.AddProperty(property);

        return new PropertyDto
        {
            Id = property.Id,
            Type = property.Type,
            Purpose = property.Purpose,
            CadastralNumber = property.CadastralNumber,
            Address = property.Address,
            Floors = property.Floors,
            TotalArea = property.TotalArea,
            Rooms = property.Rooms,
            CeilingHeight = property.CeilingHeight,
            Floor = property.Floor,
            HasEncumbrances = property.HasEncumbrances
        };
    }

    /// <summary>
    /// Обновляет данные объекта недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для обновления</param>
    /// <param name="propertyDto">DTO с обновленными данными объекта недвижимости</param> 
    public async Task<PropertyDto?> UpdatePropertyAsync(int id, PropertyUpdateDto propertyDto)
    {
        var existingProperty = _repository.GetPropertyById(id)
            ?? throw new EntityNotFoundException("Property", id);

        // Частичное обновление - обновляем только переданные поля
        if (propertyDto.Type.HasValue) existingProperty.Type = propertyDto.Type.Value;
        if (propertyDto.Purpose.HasValue) existingProperty.Purpose = propertyDto.Purpose.Value;
        if (!string.IsNullOrEmpty(propertyDto.CadastralNumber)) existingProperty.CadastralNumber = propertyDto.CadastralNumber;
        if (!string.IsNullOrEmpty(propertyDto.Address)) existingProperty.Address = propertyDto.Address;
        if (propertyDto.Floors.HasValue) existingProperty.Floors = propertyDto.Floors.Value;
        if (propertyDto.TotalArea.HasValue) existingProperty.TotalArea = propertyDto.TotalArea.Value;
        if (propertyDto.Rooms.HasValue) existingProperty.Rooms = propertyDto.Rooms.Value;
        if (propertyDto.CeilingHeight.HasValue) existingProperty.CeilingHeight = propertyDto.CeilingHeight.Value;
        if (propertyDto.Floor.HasValue) existingProperty.Floor = propertyDto.Floor.Value;
        if (propertyDto.HasEncumbrances.HasValue) existingProperty.HasEncumbrances = propertyDto.HasEncumbrances.Value;

        _repository.UpdateProperty(existingProperty);

        return new PropertyDto
        {
            Id = existingProperty.Id,
            Type = existingProperty.Type,
            Purpose = existingProperty.Purpose,
            CadastralNumber = existingProperty.CadastralNumber,
            Address = existingProperty.Address,
            Floors = existingProperty.Floors,
            TotalArea = existingProperty.TotalArea,
            Rooms = existingProperty.Rooms,
            CeilingHeight = existingProperty.CeilingHeight,
            Floor = existingProperty.Floor,
            HasEncumbrances = existingProperty.HasEncumbrances
        };
    }

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param> 
    public async Task<bool> DeletePropertyAsync(int id)
    {
        try
        {
            _repository.DeleteProperty(id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}