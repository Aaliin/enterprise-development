using AutoMapper;
using EstateAgency.Application.Dto;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
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
    /// Получает объект недвижимости по кадастровому номеру
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер</param>
    public async Task<PropertyDto?> GetByCadastralNumberAsync(string cadastralNumber)
    {
        if (string.IsNullOrWhiteSpace(cadastralNumber))
            throw new ArgumentException("Cadastral number cannot be empty", nameof(cadastralNumber));

        var property = await repository.GetByCadastralNumberAsync(cadastralNumber);
        return mapper.Map<PropertyDto?>(property);
    }

    /// <summary>
    /// Получает список объектов недвижимости по типу
    /// </summary>
    /// <param name="type">Тип недвижимости</param>
    public async Task<List<PropertyDto>> GetPropertiesByTypeAsync(PropertyType type)
    {
        var properties = await repository.GetByTypeAsync(type);
        return mapper.Map<List<PropertyDto>>(properties);
    }

    /// <summary>
    /// Создает новый объект недвижимости
    /// </summary>
    /// <param name="propertyDto">DTO с данными для создания объекта недвижимости</param> 
    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto propertyDto)
    {
        ArgumentNullException.ThrowIfNull(propertyDto);

        if (!string.IsNullOrWhiteSpace(propertyDto.CadastralNumber))
        {
            var existingProperty = await repository.GetByCadastralNumberAsync(propertyDto.CadastralNumber);
            if (existingProperty != null)
                throw new InvalidOperationException($"Property with cadastral number {propertyDto.CadastralNumber} already exists");
        }

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
        ArgumentNullException.ThrowIfNull(propertyDto);

        var existingProperty = await repository.GetByIdAsync(id);
        if (existingProperty == null)
            return null;

        if (!string.IsNullOrWhiteSpace(propertyDto.CadastralNumber) &&
            propertyDto.CadastralNumber != existingProperty.CadastralNumber)
        {
            var propertyWithSameCadastral = await repository.GetByCadastralNumberAsync(propertyDto.CadastralNumber);
            if (propertyWithSameCadastral != null && propertyWithSameCadastral.Id != id)
                throw new InvalidOperationException($"Another property with cadastral number {propertyDto.CadastralNumber} already exists");
        }

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

    /// <summary>
    /// Проверяет существование объекта недвижимости с указанным кадастровым номером
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер для проверки</param>
    public async Task<bool> PropertyExistsByCadastralNumberAsync(string cadastralNumber)
    {
        if (string.IsNullOrWhiteSpace(cadastralNumber))
            throw new ArgumentException("Cadastral number cannot be empty", nameof(cadastralNumber));

        return await repository.ExistsByCadastralNumberAsync(cadastralNumber);
    }
}