using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using EstateAgency.Ef.Data;
using Microsoft.EntityFrameworkCore;
using NATS.Client.Core;
using System.Text;
using System.Text.Json;

namespace EstateAgency.Api.Services;

/// <summary>
/// Сервис для потребления сообщений из NATS и сохранения данных в базу данных
/// </summary>
public class NatsConsumerService(
    INatsConnection natsConnection,
    IServiceScopeFactory scopeFactory,
    ILogger<NatsConsumerService> logger) : BackgroundService
{
    private readonly INatsConnection _natsConnection = natsConnection;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<NatsConsumerService> _logger = logger;
    private int _processedCount = 0;

    /// <summary>
    /// Потребление сообщений
    /// </summary>
    /// <param name="stoppingToken">Токен отмены для остановки сервиса</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting NATS Consumer Service");
        
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        try
        {
            var messages = _natsConnection.SubscribeAsync<byte[]>(subject: "estateagency.requests.create", cancellationToken: stoppingToken);

            _logger.LogInformation("Subscribed to 'estateagency.requests.create'");

            await foreach (var msg in messages.WithCancellation(stoppingToken))
            {
                if (msg.Data == null || msg.Data.Length == 0)
                {
                    _logger.LogWarning("Received message with null Data");
                    continue;
                }

                await ProcessMessageAsync(msg, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "NATS Consumer Service error");
        }
    }

    /// <summary>
    /// Обрабатывает отдельное сообщение из NATS
    /// </summary>
    /// <param name="msg">Сообщение из NATS</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    private async Task ProcessMessageAsync(NatsMsg<byte[]> msg, CancellationToken cancellationToken)
    {
        try
        {
            if (msg.Data == null || msg.Data.Length == 0)
            {
                _logger.LogWarning("Message Data is null or empty in ProcessMessageAsync");
                return;
            }

            var messageText = Encoding.UTF8.GetString(msg.Data);
            _logger.LogDebug("Received message: {Message}",
                messageText.Length > 100 ? string.Concat(messageText.AsSpan(0, 100), "...") : messageText);

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EstateAgencyDbContext>();

            await ProcessJsonMessage(messageText, dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
        }
    }

    /// <summary>
    /// Обрабатывает JSON сообщение и сохраняет данные в базу данных
    /// </summary>
    /// <param name="json">JSON строка с данными</param>
    /// <param name="dbContext">Контекст базы данных для сохранения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    private async Task ProcessJsonMessage(string json, EstateAgencyDbContext dbContext, CancellationToken cancellationToken)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(json);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("amount", out var amountProp) ||
                !root.TryGetProperty("type", out var typeProp))
            {
                _logger.LogWarning("Invalid message format - missing required fields");
                return;
            }

            var amount = amountProp.GetDecimal();
            var type = typeProp.GetInt32(); 
            var createdDate = root.TryGetProperty("createdDate", out var dateProp)
                ? dateProp.GetDateTime()
                : DateTime.UtcNow;

            Client? client = null;
            if (root.TryGetProperty("client", out var clientProp))
            {
                client = await ProcessClientAsync(dbContext, clientProp, cancellationToken);
            }

            Property? property = null;
            if (root.TryGetProperty("property", out var propertyProp))
            {
                property = await ProcessPropertyAsync(dbContext, propertyProp, cancellationToken);
            }

            if (client != null && property != null)
            {
                var request = new Request
                {
                    ClientId = client.Id,
                    PropertyId = property.Id,
                    Type = type == 0 ? RequestType.Purchase : RequestType.Sale,
                    Amount = amount,
                    CreatedDate = createdDate
                };

                dbContext.Requests.Add(request);
                await dbContext.SaveChangesAsync(cancellationToken);

                Interlocked.Increment(ref _processedCount);
                _logger.LogInformation("Saved request #{Id} for client {Name} (Total: {Count})",
                    request.Id, client.FullName, _processedCount);
            }
            else
            {
                _logger.LogWarning("Cannot save request - client or property is null");
            }
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "JSON parsing error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing JSON message");
        }
    }

    /// <summary>
    /// Обрабатывает данные клиента из JSON и сохраняет/обновляет в базе данных
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="clientJson">JSON элемент с данными клиента</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    private static async Task<Client?> ProcessClientAsync(
        EstateAgencyDbContext dbContext,
        JsonElement clientJson,
        CancellationToken cancellationToken)
    {
        if (!clientJson.TryGetProperty("passportNumber", out var passportProp))
            return null;

        var passportNumber = passportProp.GetString();
        if (string.IsNullOrEmpty(passportNumber))
            return null;

        var client = await dbContext.Clients
            .FirstOrDefaultAsync(c => c.PassportNumber == passportNumber, cancellationToken);

        if (client == null &&
            clientJson.TryGetProperty("fullName", out var nameProp) &&
            clientJson.TryGetProperty("phoneNumber", out var phoneProp))
        {
            client = new Client
            {
                FullName = nameProp.GetString() ?? "Unknown",
                PassportNumber = passportNumber,
                PhoneNumber = phoneProp.GetString() ?? "Unknown"
            };

            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return client;
    }

    /// <summary>
    /// Обрабатывает данные недвижимости из JSON и сохраняет/обновляет в базе данных
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="propertyJson">JSON элемент с данными недвижимости</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    private static async Task<Property?> ProcessPropertyAsync(
        EstateAgencyDbContext dbContext,
        JsonElement propertyJson,
        CancellationToken cancellationToken)
    {
        if (!propertyJson.TryGetProperty("cadastralNumber", out var cadastralProp))
            return null;

        var cadastralNumber = cadastralProp.GetString();
        if (string.IsNullOrEmpty(cadastralNumber))
            return null;

        var property = await dbContext.Properties
            .FirstOrDefaultAsync(p => p.CadastralNumber == cadastralNumber, cancellationToken);

        if (property == null)
        {
            property = new Property
            {
                Type = propertyJson.TryGetProperty("type", out var typeProp)
                    ? (PropertyType)typeProp.GetInt32()
                    : PropertyType.Apartment,
                Purpose = propertyJson.TryGetProperty("purpose", out var purposeProp)
                    ? (PropertyPurpose)purposeProp.GetInt32()
                    : PropertyPurpose.Residential,
                CadastralNumber = cadastralNumber,
                Address = propertyJson.TryGetProperty("address", out var addrProp)
                    ? addrProp.GetString() ?? "Unknown"
                    : "Unknown",
                Floors = propertyJson.TryGetProperty("floors", out var floorsProp)
                    ? floorsProp.GetInt32()
                    : 1,
                TotalArea = propertyJson.TryGetProperty("totalArea", out var areaProp)
                    ? areaProp.GetDecimal()
                    : 0,
                Rooms = propertyJson.TryGetProperty("rooms", out var roomsProp)
                    ? roomsProp.GetInt32()
                    : 1,
                CeilingHeight = propertyJson.TryGetProperty("ceilingHeight", out var heightProp)
                    ? heightProp.GetDecimal()
                    : null,
                Floor = propertyJson.TryGetProperty("floor", out var floorProp)
                    ? floorProp.GetInt32()
                    : null,
                HasEncumbrances = propertyJson.TryGetProperty("hasEncumbrances", out var encProp)
                    && encProp.GetBoolean()
            };

            dbContext.Properties.Add(property);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return property;
    }

    /// <summary>
    /// Метод остановки сервиса с логгированием статистики
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping NATS Consumer Service. Processed: {Count}", _processedCount);
        await base.StopAsync(cancellationToken);
    }
}