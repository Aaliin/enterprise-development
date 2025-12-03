using EstateAgency.Application.Dto;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для управления заявками риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RequestsController(IRequestService requestService, ILogger<RequestsController> logger) : ControllerBase
{
    /// <summary>
    /// Получает список всех заявок
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<RequestDto>>> GetRequests()
    {
        try
        {
            logger.LogInformation("Getting list of all requests");
            var requests = await requestService.GetAllRequestsAsync();
            logger.LogInformation("Successfully retrieved {Count} requests", requests.Count);
            return Ok(requests);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting list of requests");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving requests",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> GetRequest(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("Attempt to get request with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Request ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting request with ID: {Id}", id);
            var request = await requestService.GetRequestByIdAsync(id);

            if (request is null)
            {
                logger.LogWarning("Request with ID {Id} not found", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Request with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully retrieved request with ID: {Id}", id);
            return Ok(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting request with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving the request",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Создает новую заявку
    /// </summary>
    /// <param name="requestDto">Данные для создания заявки</param>
    [HttpPost]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> CreateRequest(CreateRequestDto requestDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid data while creating request");
                return ValidationProblem(ModelState);
            }

            logger.LogInformation("Creating new request");
            var request = await requestService.CreateRequestAsync(requestDto);

            logger.LogInformation("Successfully created request with ID: {Id}", request.Id);
            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Argument error while creating request");
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Operation error while creating request");
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating request");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while creating the request",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Обновляет данные заявки
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="requestDto">Обновленные данные заявки</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> UpdateRequest(int id, CreateRequestDto requestDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid data while updating request with ID: {Id}", id);
                return ValidationProblem(ModelState);
            }

            if (id <= 0)
            {
                logger.LogWarning("Attempt to update request with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Request ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Updating request with ID: {Id}", id);
            var request = await requestService.UpdateRequestAsync(id, requestDto);

            if (request is null)
            {
                logger.LogWarning("Request with ID {Id} not found for update", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Request with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully updated request with ID: {Id}", id);
            return Ok(request);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Argument error while updating request with ID: {Id}", id);
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Operation error while updating request with ID: {Id}", id);
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating request with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while updating the request",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("Attempt to delete request with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Request ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Deleting request with ID: {Id}", id);
            var result = await requestService.DeleteRequestAsync(id);

            if (!result)
            {
                logger.LogWarning("Request with ID {Id} not found for deletion", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Request with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully deleted request with ID: {Id}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting request with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while deleting the request",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата</param>
    /// <param name="endDate">Конечная дата</param>
    [HttpGet("analytics/sellers-by-period")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClientDto>>> GetSellersByPeriod(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
            {
                logger.LogWarning("Invalid date range: startDate {StartDate} > endDate {EndDate}", startDate, endDate);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Start date must be before or equal to end date",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting sellers by period: {StartDate} to {EndDate}", startDate, endDate);
            var sellers = await requestService.GetSellersByPeriodAsync(startDate, endDate);
            logger.LogInformation("Successfully retrieved {Count} sellers for the period", sellers.Count);
            return Ok(sellers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting sellers by period: {StartDate} to {EndDate}", startDate, endDate);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving sellers by period",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получить топ покупателей по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество топ клиентов (по умолчанию 5)</param>
    [HttpGet("analytics/top-buyers")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClientDto>>> GetTopBuyers([FromQuery] int topCount = 5)
    {
        try
        {
            if (topCount <= 0)
            {
                logger.LogWarning("Invalid topCount value: {TopCount}", topCount);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Top count must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting top {TopCount} buyers", topCount);
            var topBuyers = await requestService.GetTopBuyersAsync(topCount);
            logger.LogInformation("Successfully retrieved {Count} top buyers", topBuyers.Count);
            return Ok(topBuyers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting top buyers with count: {TopCount}", topCount);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving top buyers",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получить топ продавцов по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество топ клиентов (по умолчанию 5)</param>
    [HttpGet("analytics/top-sellers")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClientDto>>> GetTopSellers([FromQuery] int topCount = 5)
    {
        try
        {
            if (topCount <= 0)
            {
                logger.LogWarning("Invalid topCount value: {TopCount}", topCount);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Top count must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting top {TopCount} sellers", topCount);
            var topSellers = await requestService.GetTopSellersAsync(topCount);
            logger.LogInformation("Successfully retrieved {Count} top sellers", topSellers.Count);
            return Ok(topSellers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting top sellers with count: {TopCount}", topCount);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving top sellers",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получить количество заявок по типам недвижимости
    /// </summary>
    [HttpGet("analytics/requests-by-property-type")]
    [ProducesResponseType(typeof(List<(PropertyType Type, int Count)>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<(PropertyType Type, int Count)>>> GetRequestsByPropertyType()
    {
        try
        {
            logger.LogInformation("Getting requests count by property type");
            var counts = await requestService.GetRequestsCountByPropertyTypeAsync();
            logger.LogInformation("Successfully retrieved counts for {Count} property types", counts.Count);
            return Ok(counts);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting requests count by property type");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving requests count by property type",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получить клиентов с заявками минимальной стоимости
    /// </summary>
    [HttpGet("analytics/clients-with-min-amount")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClientDto>>> GetClientsWithMinAmount()
    {
        try
        {
            logger.LogInformation("Getting clients with minimum amount requests");
            var clients = await requestService.GetClientsWithMinAmountRequestsAsync();
            logger.LogInformation("Successfully retrieved {Count} clients with minimum amount", clients.Count);
            return Ok(clients);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting clients with minimum amount requests");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving clients with minimum amount",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получить клиентов, ищущих недвижимость заданного типа
    /// </summary>
    /// <param name="propertyType">Тип недвижимости</param>
    [HttpGet("analytics/clients-by-property-type/{propertyType}")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClientDto>>> GetClientsByPropertyType(PropertyType propertyType)
    {
        try
        {
            logger.LogInformation("Getting clients by property type: {PropertyType}", propertyType);
            var clients = await requestService.GetClientsByPropertyTypeAsync(propertyType);
            logger.LogInformation("Successfully retrieved {Count} clients for property type {PropertyType}",
                clients.Count, propertyType);
            return Ok(clients);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting clients by property type: {PropertyType}", propertyType);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving clients by property type",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }
}