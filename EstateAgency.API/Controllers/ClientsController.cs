using EstateAgency.Application.DTOs.Clients;
using EstateAgency.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.API.Controllers;

/// <summary>
/// Контроллер для управления клиентами риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientService clientService) : ControllerBase
{
    private readonly IClientService _clientService = clientService;

    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
    {
        var clients = await _clientService.GetAllClientsAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetClient(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        return client is null ? NotFound($"Client with ID {id} not found") : Ok(client);
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="clientDto">Данные для создания клиента</param>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateClient(ClientCreateDto clientDto)
    {
        try
        {
            var client = await _clientService.CreateClientAsync(clientDto);
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the client");
        }
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="clientDto">Обновленные данные клиента</param>
    [HttpPut("{id}")]
    public async Task<ActionResult<ClientDto>> UpdateClient(int id, ClientUpdateDto clientDto)
    {
        try
        {
            var client = await _clientService.UpdateClientAsync(id, clientDto);
            return client is null ? NotFound($"Client with ID {id} not found") : Ok(client);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the client");
        }
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            var result = await _clientService.DeleteClientAsync(id);
            return result ? NoContent() : NotFound($"Client with ID {id} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the client");
        }
    }
}