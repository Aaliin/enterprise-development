using EstateAgency.Domain;
using EstateAgency.Domain.Enum;
using EstateAgency.Domain.Data;
using Newtonsoft.Json.Linq;

namespace EstateAgency.Tests;

/// <summary>
/// Содержит тесты для проверки запросов к исходным данным о недвижимости
/// </summary>
public class EstateAgencyTests
{
    private readonly List<Client> _clients;
    private readonly List<Property> _properties;
    private readonly List<Request> _requests;

    /// <summary>
    /// Загружает тестовые данные из DataSeeder и устанавливает связи между объектами
    /// </summary>
    public EstateAgencyTests()
    {
        var testData = DataSeeder.GetCompleteTestData();
        _clients = testData.clients;
        _properties = testData.properties;
        _requests = testData.requests;

        foreach (var request in _requests)
        {
            request.Client = _clients.First(c => c.Id == request.ClientId);
            request.Property = _properties.First(p => p.Id == request.PropertyId);

            request.Client.Requests.Add(request);
            request.Property.Requests.Add(request);
        }
    }

    /// <summary>
    /// Тест 1: Вывести всех продавцов, оставивших заявки за заданный период
    /// </summary>
    [Fact]
    public void GetSellersByPeriodReturnsCorrectSellers()
    {
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 2, 28);

        var sellers = _requests
            .Where(r => r.Type == RequestType.Sale &&
                       r.CreatedDate >= startDate &&
                       r.CreatedDate <= endDate)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(2, sellers.Count);
        Assert.Equal("Иванов Иван Иванович", sellers[0].FullName);
        Assert.Equal("Сидоров Алексей Петрович", sellers[1].FullName);
    }

    /// <summary>
    /// Тест 2: Вывести топ 5 клиентов по количеству заявок (отдельно на покупку и продажу)
    /// </summary>
    [Fact]
    public void GetTopClientsReturnsCorrectTopLists()
    {
        var topBuyers = _requests
            .Where(r => r.Type == RequestType.Purchase)
            .GroupBy(r => r.Client)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Select(x => x.Client)
            .ToList();

        var topSellers = _requests
            .Where(r => r.Type == RequestType.Sale)
            .GroupBy(r => r.Client)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Select(x => x.Client)
            .ToList();

        Assert.Equal(5, topBuyers.Count);
        Assert.Equal(5, topSellers.Count);

        var buyerNames = topBuyers.Select(c => c.FullName).ToList();
        var sellerNames = topSellers.Select(c => c.FullName).ToList();

        Assert.Contains("Петрова Анна Сергеевна", buyerNames);
        Assert.Contains("Иванов Иван Иванович", sellerNames);
    }

    /// <summary>
    /// Тест 3: Вывести информацию о количестве заявок по каждому типу недвижимости
    /// </summary>
    [Fact]
    public void GetRequestsByPropertyTypeReturnsCorrectCounts()
    {
        var requestsByType = _requests
            .GroupBy(r => r.Property.Type)
            .Select(g => new { PropertyType = g.Key, Count = g.Count() })
            .ToDictionary(x => x.PropertyType, x => x.Count);

        Assert.Equal(5, requestsByType.Count);

        var actualApartmentCount = _requests.Count(r => r.Property.Type == PropertyType.Apartment);
        var actualHouseCount = _requests.Count(r => r.Property.Type == PropertyType.House);
        var actualCommercialCount = _requests.Count(r => r.Property.Type == PropertyType.Commercial);
        var actualLandCount = _requests.Count(r => r.Property.Type == PropertyType.Land);
        var actualVillaCount = _requests.Count(r => r.Property.Type == PropertyType.Villa);

        Assert.Equal(actualApartmentCount, requestsByType[PropertyType.Apartment]);
        Assert.Equal(actualHouseCount, requestsByType[PropertyType.House]);
        Assert.Equal(actualCommercialCount, requestsByType[PropertyType.Commercial]);
        Assert.Equal(actualLandCount, requestsByType[PropertyType.Land]);
        Assert.Equal(actualVillaCount, requestsByType[PropertyType.Villa]);
    }

    /// <summary>
    /// Тест 4: Вывести информацию о клиентах, открывших заявки с минимальной стоимостью
    /// </summary>
    [Fact]
    public void GetClientsWithMinAmountRequestsReturnsCorrectClients()
    {
        var minPurchaseAmount = _requests
            .Where(r => r.Type == RequestType.Purchase)
            .Min(r => r.Amount);

        var minSaleAmount = _requests
            .Where(r => r.Type == RequestType.Sale)
            .Min(r => r.Amount);

        var clients = _requests
            .Where(r => (r.Type == RequestType.Purchase && r.Amount == minPurchaseAmount) ||
                       (r.Type == RequestType.Sale && r.Amount == minSaleAmount))
            .Select(r => r.Client)
            .Distinct()
            .ToList();

        Assert.Equal(2, clients.Count);
        Assert.Contains(clients, c => c.FullName == "Петрова Анна Сергеевна");
        Assert.Contains(clients, c => c.FullName == "Федоров Дмитрий Николаевич");
    }

    /// <summary>
    /// Тест 5: Вывести сведения о всех клиентах, ищущих недвижимость заданного типа, упорядочить по ФИО
    /// </summary>
    [Fact]
    public void GetClientsByPropertyTypeReturnsSortedClients()
    {
        var propertyType = PropertyType.Apartment;

        var clients = _requests
            .Where(r => r.Type == RequestType.Purchase &&
                       r.Property.Type == propertyType)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(3, clients.Count);
        Assert.Equal("Козлова Мария Владимировна", clients[0].FullName);
        Assert.Equal("Петрова Анна Сергеевна", clients[1].FullName);
        Assert.Equal("Сидоров Алексей Петрович", clients[2].FullName);
    }
}