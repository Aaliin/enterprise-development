using EstateAgency.Application.Dto;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;

namespace EstateAgency.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с объектами недвижимости
/// </summary>
public interface IPropertyService
{
    /// <summary>
    /// Получает список всех объектов недвижимости
    /// </summary>
    public Task<List<PropertyDto>> GetAllPropertiesAsync();

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    public Task<PropertyDto?> GetPropertyByIdAsync(int id);

    /// <summary>
    /// Находит объект недвижимости по кадастровому номеру
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер</param>
    public Task<PropertyDto?> GetByCadastralNumberAsync(string cadastralNumber);

    /// <summary>
    /// Получает список объектов недвижимости по типу
    /// </summary>
    /// <param name="type">Тип недвижимости</param>
    public Task<List<PropertyDto>> GetPropertiesByTypeAsync(PropertyType type);

    /// <summary>
    /// Создает новый объект недвижимости
    /// </summary>
    /// <param name="propertyDto">DTO для создания объекта</param>
    public Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto propertyDto);

    /// <summary>
    /// Обновляет данные объекта недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <param name="propertyDto">DTO с обновленными данными</param>
    public Task<PropertyDto?> UpdatePropertyAsync(int id, CreatePropertyDto propertyDto);

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    public Task<bool> DeletePropertyAsync(int id);

    /// <summary>
    /// Проверяет существование объекта недвижимости с указанным кадастровым номером
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер для проверки</param>
    public Task<bool> PropertyExistsByCadastralNumberAsync(string cadastralNumber);
}