using EstateAgency.Application.Dto;
using EstateAgency.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Контроллер для управления клиентами риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientsController(IClientService clientService, ILogger<ClientsController> logger) : ControllerBase
{
    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClientDto>>> GetClients()
    {
        try
        {
            logger.LogInformation("Getting list of all clients");
            var clients = await clientService.GetAllClientsAsync();
            logger.LogInformation("Successfully retrieved {Count} clients", clients.Count);
            return Ok(clients);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting list of clients");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving clients",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> GetClient(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("Attempt to get client with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Client ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting client with ID: {Id}", id);
            var client = await clientService.GetClientByIdAsync(id);

            if (client is null)
            {
                logger.LogWarning("Client with ID {Id} not found", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Client with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully retrieved client with ID: {Id}", id);
            return Ok(client);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting client with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving the client",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Получает клиента по номеру паспорта
    /// </summary>
    /// <param name="passportNumber">Номер паспорта</param>
    [HttpGet("by-passport/{passportNumber}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> GetClientByPassport(string passportNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(passportNumber))
            {
                logger.LogWarning("Attempt to get client with empty passport number");
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Passport number cannot be empty",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Getting client by passport number: {PassportNumber}", passportNumber);
            var client = await clientService.GetClientByPassportAsync(passportNumber);

            if (client is null)
            {
                logger.LogWarning("Client with passport number {PassportNumber} not found", passportNumber);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Client with passport number {passportNumber} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully retrieved client by passport number: {PassportNumber}", passportNumber);
            return Ok(client);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting client by passport number: {PassportNumber}", passportNumber);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving the client by passport number",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Проверяет, существует ли клиент с указанным номером паспорта exists
    /// </summary>
    /// <param name="passportNumber">Номер паспорта</param>
    [HttpGet("exists/passport/{passportNumber}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> CheckPassportExists(string passportNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(passportNumber))
            {
                logger.LogWarning("Attempt to check existence with empty passport number");
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Passport number cannot be empty",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Checking if client with passport number exists: {PassportNumber}", passportNumber);
            var exists = await clientService.ClientExistsByPassportAsync(passportNumber);

            logger.LogInformation("Passport number {PassportNumber} exists: {Exists}", passportNumber, exists);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while checking passport number existence: {PassportNumber}", passportNumber);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while checking passport number existence",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="clientDto">Данные для создания клиента</param>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> CreateClient(CreateClientDto clientDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid data while creating client");
                return ValidationProblem(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(clientDto.PassportNumber))
            {
                var exists = await clientService.ClientExistsByPassportAsync(clientDto.PassportNumber);
                if (exists)
                {
                    logger.LogWarning("Attempt to create client with duplicate passport number: {PassportNumber}",
                        clientDto.PassportNumber);
                    return Conflict(new ProblemDetails
                    {
                        Title = "Conflict",
                        Detail = $"Client with passport number {clientDto.PassportNumber} already exists",
                        Status = StatusCodes.Status409Conflict
                    });
                }
            }

            logger.LogInformation("Creating new client");
            var client = await clientService.CreateClientAsync(clientDto);

            logger.LogInformation("Successfully created client with ID: {Id}", client.Id);
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Argument error while creating client");
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Operation error while creating client");
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating client");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while creating the client",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="clientDto">Обновленные данные клиента</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> UpdateClient(int id, CreateClientDto clientDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid data while updating client with ID: {Id}", id);
                return ValidationProblem(ModelState);
            }

            if (id <= 0)
            {
                logger.LogWarning("Attempt to update client with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Client ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var existingClient = await clientService.GetClientByIdAsync(id);
            if (existingClient is null)
            {
                logger.LogWarning("Client with ID {Id} not found for update", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Client with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            if (!string.IsNullOrWhiteSpace(clientDto.PassportNumber) &&
                clientDto.PassportNumber != existingClient.PassportNumber)
            {
                var passportExists = await clientService.ClientExistsByPassportAsync(clientDto.PassportNumber);
                if (passportExists)
                {
                    logger.LogWarning("Attempt to update client ID {Id} with duplicate passport number: {PassportNumber}",
                        id, clientDto.PassportNumber);
                    return Conflict(new ProblemDetails
                    {
                        Title = "Conflict",
                        Detail = $"Another client with passport number {clientDto.PassportNumber} already exists",
                        Status = StatusCodes.Status409Conflict
                    });
                }
            }

            logger.LogInformation("Updating client with ID: {Id}", id);
            var client = await clientService.UpdateClientAsync(id, clientDto);

            if (client is null)
            {
                logger.LogWarning("Client with ID {Id} not found for update", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Client with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully updated client with ID: {Id}", id);
            return Ok(client);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Argument error while updating client with ID: {Id}", id);
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Operation error while updating client with ID: {Id}", id);
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating client with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while updating the client",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента для удаления</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("Attempt to delete client with invalid ID: {Id}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Client ID must be greater than 0",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            logger.LogInformation("Deleting client with ID: {Id}", id);
            var result = await clientService.DeleteClientAsync(id);

            if (!result)
            {
                logger.LogWarning("Client with ID {Id} not found for deletion", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Client with ID {id} not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            logger.LogInformation("Successfully deleted client with ID: {Id}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting client with ID: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while deleting the client",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }
}