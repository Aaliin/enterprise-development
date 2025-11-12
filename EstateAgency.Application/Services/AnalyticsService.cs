using EstateAgency.Application.DTOs.Analytics;
using EstateAgency.Application.Interfaces.Repositories;
using EstateAgency.Application.Interfaces.Services;
using EstateAgency.Domain.Enum;

namespace EstateAgency.Application.Services;

/// <summary>
/// Реализует бизнес-логику для генерации аналитических отчетов и статистики
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным агентства недвижимости</param>
public class AnalyticsService(IEstateAgencyRepository repository) : IAnalyticsService
{
    private readonly IEstateAgencyRepository _repository = repository;

    /// <summary>
    /// Получает продавцов и статистику их продаж за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода анализа</param>
    /// <param name="endDate">Конечная дата периода анализа</param>
    public async Task<List<SellerPeriodDto>> GetSellersByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var sellers = await _repository.GetSellersByPeriodAsync(startDate, endDate);

        return [.. sellers.Select(seller =>
        {
            var salesRequests = seller.Requests
                .Where(r => r.Type == RequestType.Sale &&
                           r.CreatedDate >= startDate &&
                           r.CreatedDate <= endDate)
                .ToList();

            return new SellerPeriodDto
            {
                ClientId = seller.Id,
                ClientName = seller.FullName,
                SalesCount = salesRequests.Count,
                TotalSalesAmount = salesRequests.Sum(r => r.Amount),
                PeriodStart = startDate,
                PeriodEnd = endDate
            };
        })
        .OrderByDescending(s => s.TotalSalesAmount)];
    }

    /// <summary>
    /// Получает топ клиентов по количеству заявок указанного типа
    /// </summary>
    /// <param name="type">Тип заявки для анализа (покупка/продажа)</param>
    /// <param name="topCount">Количество клиентов в рейтинге (по умолчанию 5)</param>
    public async Task<List<ClientAnalyticsDto>> GetTopClientsByRequestsAsync(RequestType type, int topCount = 5)
    {
        var topClients = await _repository.GetTop5ClientsByRequests(type);

        return [.. topClients.Select((t, index) => new ClientAnalyticsDto
        {
            ClientId = t.Client.Id,
            FullName = t.Client.FullName,
            PhoneNumber = t.Client.PhoneNumber,
            RequestsCount = t.Count,
            RequestType = type.ToString(),
            Rank = index + 1
        })
        .Take(topCount)];
    }

    /// <summary>
    /// Получает статистику количества заявок по типам недвижимости
    /// </summary>
    public async Task<List<PropertyTypeStatDto>> GetRequestCountByPropertyTypeAsync()
    {
        var stats = await _repository.GetRequestsByPropertyTypeAsync();
        var totalRequests = stats.Sum(s => s.Value);

        return [.. stats.Select(s => new PropertyTypeStatDto
        {
            PropertyType = s.Key.ToString(),
            RequestCount = s.Value,
            Percentage = totalRequests > 0 ? Math.Round((s.Value / (double)totalRequests) * 100, 2) : 0
        })
        .OrderByDescending(s => s.RequestCount)];
    }

    /// <summary>
    /// Получает клиентов с заявками минимальной стоимости
    /// </summary>
    public async Task<List<ClientAnalyticsDto>> GetClientsWithMinAmountRequestAsync()
    {
        var requests = await _repository.GetRequestsAsync();

        if (requests.Count == 0)
            return [];

        var minAmount = requests.Min(r => r.Amount);
        var clientsWithMinAmount = requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Client)
            .Distinct()
            .ToList();

        return [.. clientsWithMinAmount.Select(c => new ClientAnalyticsDto
        {
            ClientId = c.Id,
            FullName = c.FullName,
            PhoneNumber = c.PhoneNumber,
            RequestsCount = requests.Count(r => r.ClientId == c.Id),
            MinAmount = minAmount
        })];
    }

    /// <summary>
    /// Получает клиентов, ищущих недвижимость заданного типа
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для поиска</param>
    public async Task<List<ClientAnalyticsDto>> GetClientsSearchingPropertyTypeAsync(PropertyType propertyType)
    {
        var clients = await _repository.GetClientsByPropertyTypeAsync(propertyType);

        return [.. clients.Select(c => new ClientAnalyticsDto
        {
            ClientId = c.Id,
            FullName = c.FullName,
            PhoneNumber = c.PhoneNumber,
            TargetPropertyType = propertyType.ToString(),
            RequestsCount = c.Requests.Count(r => r.Property != null && r.Property.Type == propertyType)
        })];
    }
}