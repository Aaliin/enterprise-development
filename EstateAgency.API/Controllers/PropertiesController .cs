using EstateAgency.Domain.Enum;
using EstateAgency.Application.Dto;
using EstateAgency.Application.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для управления объектами недвижимости риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger) : ControllerBase
{
    /// <summary>
    /// Получает список всех объектов недвижимости
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PropertyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<PropertyDto>>> GetProperties()
    {
        try
        {
            logger.LogInformation("Getting list of all properties");
            var properties = await propertyService.GetAllPropertiesAsync();
            logger.LogInformation("Successfully retrieved {Count} properties", properties.Count);
            return Ok(properties);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting list of properties");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving properties",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получает объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDto>> GetProperty(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("Attempt to get property with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Property ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting property with ID: {Id}", id);
            var property = await propertyService.GetPropertyByIdAsync(id);

            if (property is null)
            {
                logger.LogWarning("Property with ID {Id} not found", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Property with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully retrieved property with ID: {Id}", id);
            return Ok(property);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting property with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving the property",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получает объект недвижимости по кадастровому номеру
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер</param>
    [HttpGet("by-cadastral/{cadastralNumber}")]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDto>> GetPropertyByCadastralNumber(string cadastralNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cadastralNumber))
            {
                logger.LogWarning("Attempt to get property with empty cadastral number");
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Cadastral number cannot be empty",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting property by cadastral number: {CadastralNumber}", cadastralNumber);
            var property = await propertyService.GetByCadastralNumberAsync(cadastralNumber);

            if (property is null)
            {
                logger.LogWarning("Property with cadastral number {CadastralNumber} not found", cadastralNumber);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Property with cadastral number {cadastralNumber} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully retrieved property by cadastral number: {CadastralNumber}", cadastralNumber);
            return Ok(property);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting property by cadastral number: {CadastralNumber}", cadastralNumber);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving the property by cadastral number",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получает список объектов недвижимости по типу
    /// </summary>
    /// <param name="type">Тип недвижимости</param>
    [HttpGet("by-type/{type}")]
    [ProducesResponseType(typeof(List<PropertyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<PropertyDto>>> GetPropertiesByType(PropertyType type)
    {
        try
        {
            logger.LogInformation("Getting properties by type: {Type}", type);
            var properties = await propertyService.GetPropertiesByTypeAsync(type);
            logger.LogInformation("Successfully retrieved {Count} properties of type {Type}", properties.Count, type);
            return Ok(properties);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting properties by type: {Type}", type);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving properties by type",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Создает новый объект недвижимости
    /// </summary>
    /// <param name="propertyDto">Данные для создания объекта недвижимости</param>
    [HttpPost]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDto>> CreateProperty(CreatePropertyDto propertyDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid data while creating property");
                return ValidationProblem(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(propertyDto.CadastralNumber))
            {
                var exists = await propertyService.PropertyExistsByCadastralNumberAsync(propertyDto.CadastralNumber);
                if (exists)
                {
                    logger.LogWarning("Attempt to create property with duplicate cadastral number: {CadastralNumber}",
                        propertyDto.CadastralNumber);
                    return Conflict(new ProblemDetails
                    {
                        Title = "Conflict",
                        Detail = $"Property with cadastral number {propertyDto.CadastralNumber} already exists",
                        Status = StatusCodes.Status409Conflict
                    });
                }
            }

            logger.LogInformation("Creating new property");
            var property = await propertyService.CreatePropertyAsync(propertyDto);

            logger.LogInformation("Successfully created property with ID: {Id}", property.Id);
            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Argument error while creating property");
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Operation error while creating property");
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating property");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while creating the property",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Обновляет данные объекта недвижимости
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости</param>
    /// <param name="propertyDto">Обновленные данные объекта недвижимости</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDto>> UpdateProperty(int id, CreatePropertyDto propertyDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid data while updating property with ID: {Id}", id);
                return ValidationProblem(ModelState);
            }

            if (id <= 0)
            {
                logger.LogWarning("Attempt to update property with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Property ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var existingProperty = await propertyService.GetPropertyByIdAsync(id);
            if (existingProperty is null)
            {
                logger.LogWarning("Property with ID {Id} not found for update", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Property with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            if (!string.IsNullOrWhiteSpace(propertyDto.CadastralNumber) &&
                propertyDto.CadastralNumber != existingProperty.CadastralNumber)
            {
                var cadastralExists = await propertyService.PropertyExistsByCadastralNumberAsync(propertyDto.CadastralNumber);
                if (cadastralExists)
                {
                    logger.LogWarning("Attempt to update property ID {Id} with duplicate cadastral number: {CadastralNumber}",
                        id, propertyDto.CadastralNumber);
                    return Conflict(new ProblemDetails
                    {
                        Title = "Conflict",
                        Detail = $"Another property with cadastral number {propertyDto.CadastralNumber} already exists",
                        Status = StatusCodes.Status409Conflict
                    });
                }
            }

            logger.LogInformation("Updating property with ID: {Id}", id);
            var property = await propertyService.UpdatePropertyAsync(id, propertyDto);

            if (property is null)
            {
                logger.LogWarning("Property with ID {Id} not found for update", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Property with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully updated property with ID: {Id}", id);
            return Ok(property);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Argument error while updating property with ID: {Id}", id);
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Operation error while updating property with ID: {Id}", id);
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating property with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while updating the property",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Удаляет объект недвижимости по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта недвижимости для удаления</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("Attempt to delete property with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Property ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Deleting property with ID: {Id}", id);
            var result = await propertyService.DeletePropertyAsync(id);

            if (!result)
            {
                logger.LogWarning("Property with ID {Id} not found for deletion", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Property with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully deleted property with ID: {Id}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting property with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while deleting the property",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Проверяет, существует ли объект недвижимости с указанным кадастровым номером
    /// </summary>
    /// <param name="cadastralNumber">Кадастровый номер для проверки</param>
    [HttpGet("exists/cadastral/{cadastralNumber}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> CheckCadastralNumberExists(string cadastralNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cadastralNumber))
            {
                logger.LogWarning("Attempt to check existence with empty cadastral number");
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Cadastral number cannot be empty",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Checking if property with cadastral number exists: {CadastralNumber}", cadastralNumber);
            var exists = await propertyService.PropertyExistsByCadastralNumberAsync(cadastralNumber);

            logger.LogInformation("Cadastral number {CadastralNumber} exists: {Exists}", cadastralNumber, exists);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while checking cadastral number existence: {CadastralNumber}", cadastralNumber);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while checking cadastral number existence",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

}