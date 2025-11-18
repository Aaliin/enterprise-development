using EstateAgency.Application.DTOs;
using EstateAgency.Application.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.API.Controllers;

/// <summary>
/// Контроллер для управления объектами недвижимости риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PropertiesController(IPropertyService propertyService) : ControllerBase
{
    private readonly IPropertyService _propertyService = propertyService;

    /// <summary>
    /// Получает список всех объектов недвижимости
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties()
    {
        var properties = await _propertyService.GetAllPropertiesAsync();
        return Ok(properties);
    }

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDto>> GetProperty(int id)
    {
        var property = await _propertyService.GetPropertyByIdAsync(id);
        return property is null ? NotFound($"Property with ID {id} not found") : Ok(property);
    }

    /// <summary>
    /// Создает новый объект недвижимости
    /// </summary>
    /// <param name="propertyDto">Данные для создания объекта недвижимости</param>
    [HttpPost]
    public async Task<ActionResult<PropertyDto>> CreateProperty(CreatePropertyDto propertyDto)
    {
        try
        {
            var property = await _propertyService.CreatePropertyAsync(propertyDto);
            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the property");
        }
    }

    /// <summary>
    /// Обновляет данные объекта недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    /// <param name="propertyDto">Обновленные данные объекта недвижимости</param>
    [HttpPut("{id}")]
    public async Task<ActionResult<PropertyDto>> UpdateProperty(int id, CreatePropertyDto propertyDto)
    {
        try
        {
            var property = await _propertyService.UpdatePropertyAsync(id, propertyDto);
            return property is null ? NotFound($"Property with ID {id} not found") : Ok(property);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the property");
        }
    }

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        try
        {
            var result = await _propertyService.DeletePropertyAsync(id);
            return result ? NoContent() : NotFound($"Property with ID {id} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the property");
        }
    }
}