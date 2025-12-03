using EstateAgency.Application.DTOs;
using EstateAgency.Application.Interfaces;
using EstateAgency.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.API.Controllers;

/// <summary>
/// Контроллер для управления заявками риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RequestsController(IRequestService requestService) : ControllerBase
{
    private readonly IRequestService _requestService = requestService;

    /// <summary>
    /// Получает список всех заявок
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<RequestDto>>> GetRequests()
    {
        var requests = await _requestService.GetAllRequestsAsync();
        return Ok(requests);
    }

    /// <summary>
    /// Получает заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<RequestDto>> GetRequest(int id)
    {
        if (id <= 0)
        {
            ModelState.AddModelError(nameof(id), "Request ID must be greater than 0");
            return BadRequest(ModelState);
        }
        var request = await _requestService.GetRequestByIdAsync(id);
        return request is null ? NotFound($"Request with ID {id} not found") : Ok(request);
    }

    /// <summary>
    /// Создает новую заявку
    /// </summary>
    /// <param name="requestDto">Данные для создания заявки</param>
    [HttpPost]
    public async Task<ActionResult<RequestDto>> CreateRequest(CreateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var request = await _requestService.CreateRequestAsync(requestDto);
            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Conflict(ModelState);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the request");
        }
    }

    /// <summary>
    /// Обновляет данные заявки
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="requestDto">Обновленные данные заявки</param>
    [HttpPut("{id}")]
    public async Task<ActionResult<RequestDto>> UpdateRequest(int id, CreateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id <= 0)
        {
            ModelState.AddModelError(nameof(id), "Request ID must be greater than 0");
            return BadRequest(ModelState);
        }
        try
        {
            var request = await _requestService.UpdateRequestAsync(id, requestDto);
            return request is null ? NotFound($"Request with ID {id} not found") : Ok(request);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Conflict(ModelState);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the request");
        }
    }

    /// <summary>
    /// Удаляет заявку по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки для удаления</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        if (id <= 0)
        {
            ModelState.AddModelError(nameof(id), "Request ID must be greater than 0");
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _requestService.DeleteRequestAsync(id);
            return result ? NoContent() : NotFound($"Request with ID {id} not found");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the request");
        }
    }

    /// <summary>
    /// Получить продавцов за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата</param>
    /// <param name="endDate">Конечная дата</param>
    [HttpGet("analytics/sellers-by-period")]
    public async Task<ActionResult<List<ClientDto>>> GetSellersByPeriod(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            ModelState.AddModelError(nameof(startDate), "Start date cannot be after end date");
            return BadRequest(ModelState);
        }
        var sellers = await _requestService.GetSellersByPeriodAsync(startDate, endDate);
        return Ok(sellers);
    }

    /// <summary>
    /// Получить топ покупателей по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество топ клиентов (по умолчанию 5)</param>
    [HttpGet("analytics/top-buyers")]
    public async Task<ActionResult<List<ClientDto>>> GetTopBuyers([FromQuery] int topCount = 5)
    {
        if (topCount <= 0 || topCount > 100)
        {
            ModelState.AddModelError(nameof(topCount), "Top count must be between 1 and 100");
            return BadRequest(ModelState);
        }
        var buyers = await _requestService.GetTopBuyersAsync(topCount);
        return Ok(buyers);
    }

    /// <summary>
    /// Получить топ продавцов по количеству заявок
    /// </summary>
    /// <param name="topCount">Количество топ клиентов (по умолчанию 5)</param>
    [HttpGet("analytics/top-sellers")]
    public async Task<ActionResult<List<ClientDto>>> GetTopSellers([FromQuery] int topCount = 5)
    {
        if (topCount <= 0 || topCount > 100)
        {
            ModelState.AddModelError(nameof(topCount), "Top count must be between 1 and 100");
            return BadRequest(ModelState);
        }
        var sellers = await _requestService.GetTopSellersAsync(topCount);
        return Ok(sellers);
    }

    /// <summary>
    /// Получить количество заявок по типам недвижимости
    /// </summary>
    [HttpGet("analytics/requests-by-property-type")]
    public async Task<ActionResult<List<(PropertyType Type, int Count)>>> GetRequestsByPropertyType()
    {
        var counts = await _requestService.GetRequestsCountByPropertyTypeAsync();
        return Ok(counts);
    }

    /// <summary>
    /// Получить клиентов с заявками минимальной стоимости
    /// </summary>
    [HttpGet("analytics/clients-with-min-amount")]
    public async Task<ActionResult<List<ClientDto>>> GetClientsWithMinAmount()
    {
        var clients = await _requestService.GetClientsWithMinAmountRequestsAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Получить клиентов, ищущих недвижимость заданного типа
    /// </summary>
    /// <param name="propertyType">Тип недвижимости</param>
    [HttpGet("analytics/clients-by-property-type/{propertyType}")]
    public async Task<ActionResult<List<ClientDto>>> GetClientsByPropertyType(PropertyType propertyType)
    {
        if (!Enum.IsDefined(typeof(PropertyType), propertyType))
        {
            ModelState.AddModelError(nameof(propertyType), "Invalid property type");
            return BadRequest(ModelState);
        }
        var clients = await _requestService.GetClientsByPropertyTypeAsync(propertyType);
        return Ok(clients);
    }
}