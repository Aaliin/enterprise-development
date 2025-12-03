using AutoMapper;
using EstateAgency.Application.DTOs;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Interfaces;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику работы с каталогом недвижимости
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
/// <param name="mapper">AutoMapper для преобразования объектов</param>
public class PropertyService(IPropertyRepository repository, IMapper mapper) : IPropertyService
{
    /// <summary>
    /// Получает список всех объектов недвижимости
    /// </summary>
    public async Task<List<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = await repository.GetAllAsync();
        return mapper.Map<List<PropertyDto>>(properties);
    }

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
    {
        var property = await repository.GetByIdAsync(id);
        return mapper.Map<PropertyDto?>(property);
    }

    /// <summary>
    /// Создает новый объект недвижимости
    /// </summary>
    /// <param name="propertyDto">DTO с данными для создания объекта недвижимости</param> 
    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto propertyDto)
    {
        var property = mapper.Map<Property>(propertyDto);
        var createdProperty = await repository.AddAsync(property);
        return mapper.Map<PropertyDto>(createdProperty);
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

        mapper.Map(propertyDto, existingProperty);
        var updatedProperty = await repository.UpdateAsync(existingProperty);

        return mapper.Map<PropertyDto>(updatedProperty);
    }

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param> 
    public async Task<bool> DeletePropertyAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }
}