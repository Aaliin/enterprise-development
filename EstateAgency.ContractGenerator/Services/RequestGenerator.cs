using Bogus;
using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using Microsoft.Extensions.Logging;

namespace EstateAgency.ContractGenerator.Services;

/// <summary>
/// Генератор тестовых запросов на создание контрактов 
/// </summary>
/// <param name="logger">Логгер для записи событий генерации</param>
public class RequestGenerator : IRequestGenerator
{
    private readonly ILogger<RequestGenerator> _logger;
    private readonly Faker<Request> _requestFaker;
    private int _requestCounter = 1;

    /// <summary>
    /// Конструктор генератора запросов
    /// </summary>
    /// <param name="logger">Логгер для записи событий генерации</param>
    public RequestGenerator(ILogger<RequestGenerator> logger)
    {
        _logger = logger;

        Randomizer.Seed = new Random(8675309);

        var clientFaker = new Faker<Client>("ru")
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.FullName, f => f.Person.FullName)
            .RuleFor(c => c.PassportNumber, f => f.Random.Replace("#### ######"))
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("+7 (###) ###-##-##"));

        var propertyFaker = new Faker<Property>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Type, f => f.PickRandom<PropertyType>())
            .RuleFor(p => p.Purpose, f => f.PickRandom<PropertyPurpose>())
            .RuleFor(p => p.CadastralNumber, f => f.Random.Replace("##:##:######:####"))
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.Floors, f => f.Random.Int(1, 25))
            .RuleFor(p => p.TotalArea, f => f.Random.Decimal(30, 300))
            .RuleFor(p => p.Rooms, f => f.Random.Int(1, 6))
            .RuleFor(p => p.CeilingHeight, f => f.Random.Decimal(2.5m, 3.5m))
            .RuleFor(p => p.Floor, f => f.Random.Int(1, 25))
            .RuleFor(p => p.HasEncumbrances, f => f.Random.Bool(0.2f));

        _requestFaker = new Faker<Request>()
            .RuleFor(r => r.Id, f => _requestCounter++)
            .RuleFor(r => r.Type, f => f.PickRandom<RequestType>())
            .RuleFor(r => r.Amount, f => f.Finance.Amount(1_000_000, 50_000_000))
            .RuleFor(r => r.CreatedDate, f => f.Date.Recent(30))
            .RuleFor(r => r.Client, f =>
            {
                var client = clientFaker.Generate();
                client.Requests = [];
                return client;
            })
            .RuleFor(r => r.ClientId, (f, r) => r.Client?.Id ?? 0)
            .RuleFor(r => r.Property, f =>
            {
                var property = propertyFaker.Generate();
                property.Requests = [];
                return property;
            })
            .RuleFor(r => r.PropertyId, (f, r) => r.Property?.Id ?? 0);
    }

    /// <summary>
    /// Генерирует одиночный запрос на создание контракта
    /// </summary>
    public Request GenerateSingle()
    {
        var request = _requestFaker.Generate();
        _logger.LogDebug("Generated request #{Id} for client {ClientName}",
            request.Id, request.Client?.FullName);
        return request;
    }

    /// <summary>
    /// Генерирует пакет запросов на создание контрактов
    /// </summary>
    /// <param name="count">Количество запросов в пакете</param>
    public IEnumerable<Request> GenerateBatch(int count)
    {
        _logger.LogInformation("Generating batch of {Count} requests", count);

        for (var i = 0; i < count; i++)
        {
            yield return GenerateSingle();
        }
    }

    /// <summary>
    /// Асинхронно генерирует пакет запросов на создание контрактов
    /// </summary>
    /// <param name="count">Количество запросов в пакете</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    public async Task<IEnumerable<Request>> GenerateBatchAsync(int count, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Async generating batch of {Count} requests", count);

        var tasks = new List<Task<Request>>();

        for (var i = 0; i < count; i++)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            tasks.Add(Task.Run(GenerateSingle, cancellationToken));
        }

        return await Task.WhenAll(tasks);
    }
}