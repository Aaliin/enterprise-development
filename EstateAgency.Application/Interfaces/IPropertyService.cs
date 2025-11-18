using EstateAgency.Application.DTOs;

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
}