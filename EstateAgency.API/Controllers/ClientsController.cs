using EstateAgency.Application.DTOs;
using EstateAgency.Application.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.API.Controllers;

/// <summary>
/// Контроллер для управления клиентами риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientService clientService) : ControllerBase
{
    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetClients()
    {
        var clients = await clientService.GetAllClientsAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetClient(int id)
    {
        if (id <= 0)
        {
            ModelState.AddModelError(nameof(id), "Client ID must be greater than 0");
            return BadRequest(ModelState);
        }

        var client = await clientService.GetClientByIdAsync(id);
        return client is null ? NotFound($"Client with ID {id} not found") : Ok(client);
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="clientDto">Данные для создания клиента</param>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateClient(CreateClientDto clientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var client = await clientService.CreateClientAsync(clientDto);
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
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
    public async Task<ActionResult<ClientDto>> UpdateClient(int id, CreateClientDto clientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id <= 0)
        {
            ModelState.AddModelError(nameof(id), "Client ID must be greater than 0");
            return BadRequest(ModelState);
        }
        try
        {
            var client = await clientService.UpdateClientAsync(id, clientDto);
            return client is null ? NotFound($"Client with ID {id} not found") : Ok(client);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
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
        if (id <= 0)
        {
            ModelState.AddModelError(nameof(id), "Client ID must be greater than 0");
            return BadRequest(ModelState);
        }
        try
        {
            var result = await clientService.DeleteClientAsync(id);
            return result ? NoContent() : NotFound($"Client with ID {id} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the client");
        }
    }
}