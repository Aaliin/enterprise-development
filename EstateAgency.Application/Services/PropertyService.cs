using EstateAgency.Application.DTOs;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interfaces;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с каталогом недвижимости
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
public class PropertyService(IPropertyRepository repository) : IPropertyService
{
    /// <summary>
    /// Получает список всех объектов недвижимости
    /// </summary>
    public async Task<List<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = await repository.GetAllAsync();
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
        var property = await repository.GetByIdAsync(id);
        if (property == null) return null; 
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
    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto propertyDto)
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

        var createdProperty = await repository.AddAsync(property);

        return new PropertyDto
        {
            Id = createdProperty.Id,
            Type = createdProperty.Type,
            Purpose = createdProperty.Purpose,
            CadastralNumber = createdProperty.CadastralNumber,
            Address = createdProperty.Address,
            Floors = createdProperty.Floors,
            TotalArea = createdProperty.TotalArea,
            Rooms = createdProperty.Rooms,
            CeilingHeight = createdProperty.CeilingHeight,
            Floor = createdProperty.Floor,
            HasEncumbrances = createdProperty.HasEncumbrances
        };
    }

    /// <summary>
    /// Обновляет данные объекта недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для обновления</param>
    /// <param name="propertyDto">DTO с обновленными данными объекта недвижимости</param> 
    public async Task<PropertyDto?> UpdatePropertyAsync(int id, CreatePropertyDto propertyDto)
    {
        var existingProperty = await repository.GetByIdAsync(id);
        if (existingProperty == null) return null;

        existingProperty.Type = propertyDto.Type;
        existingProperty.Purpose = propertyDto.Purpose;
        existingProperty.CadastralNumber = propertyDto.CadastralNumber;
        existingProperty.Address = propertyDto.Address;
        existingProperty.Floors = propertyDto.Floors;
        existingProperty.TotalArea = propertyDto.TotalArea;
        existingProperty.Rooms = propertyDto.Rooms;
        existingProperty.CeilingHeight = propertyDto.CeilingHeight;
        existingProperty.Floor = propertyDto.Floor;
        existingProperty.HasEncumbrances = propertyDto.HasEncumbrances;

        var updatedProperty = await repository.UpdateAsync(existingProperty); 

        return new PropertyDto
        {
            Id = updatedProperty.Id,
            Type = updatedProperty.Type,
            Purpose = updatedProperty.Purpose,
            CadastralNumber = updatedProperty.CadastralNumber,
            Address = updatedProperty.Address,
            Floors = updatedProperty.Floors,
            TotalArea = updatedProperty.TotalArea,
            Rooms = updatedProperty.Rooms,
            CeilingHeight = updatedProperty.CeilingHeight,
            Floor = updatedProperty.Floor,
            HasEncumbrances = updatedProperty.HasEncumbrances
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
            var createdProperty = await repository.DeleteAsync(id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}