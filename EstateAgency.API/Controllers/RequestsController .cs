using EstateAgency.Application.DTOs.Requests;
using EstateAgency.Application.Interfaces.Services;
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
    public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequests()
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
        var request = await _requestService.GetRequestByIdAsync(id);
        return request is null ? NotFound($"Request with ID {id} not found") : Ok(request);
    }

    /// <summary>
    /// Создает новую заявку
    /// </summary>
    /// <param name="requestDto">Данные для создания заявки</param>
    [HttpPost]
    public async Task<ActionResult<RequestDto>> CreateRequest(RequestCreateDto requestDto)
    {
        try
        {
            var request = await _requestService.CreateRequestAsync(requestDto);
            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
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
    public async Task<ActionResult<RequestDto>> UpdateRequest(int id, RequestUpdateDto requestDto)
    {
        try
        {
            var request = await _requestService.UpdateRequestAsync(id, requestDto);
            return request is null ? NotFound($"Request with ID {id} not found") : Ok(request);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
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
        try
        {
            var result = await _requestService.DeleteRequestAsync(id);
            return result ? NoContent() : NotFound($"Request with ID {id} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the request");
        }
    }
}