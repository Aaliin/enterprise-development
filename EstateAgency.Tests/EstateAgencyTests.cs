using EstateAgency.Domain.Enum;

namespace EstateAgency.Tests;

/// <summary>
/// Содержит тесты для проверки запросов к исходным данным о недвижимости
/// </summary>
public class EstateAgencyTests(TestDataFixture _fixture) : IClassFixture<TestDataFixture>
{
    private const string IvanovFullName = "Иванов Иван Иванович";
    private const string PetrovaFullName = "Петрова Анна Сергеевна";
    private const string SidorovFullName = "Сидоров Алексей Петрович";
    private const string KozlovaFullName = "Козлова Мария Владимировна";
    private const string FedorovFullName = "Федоров Дмитрий Николаевич";

    /// <summary>
    /// Тест 1: Вывести всех продавцов, оставивших заявки за заданный период
    /// </summary>
    [Fact]
    public void GetSellersByPeriodReturnsCorrectSellers()
    {
        var expectedCount = 2;
        var expectedFirstSeller = IvanovFullName;
        var expectedSecondSeller = SidorovFullName;
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 2, 28);

        var sellers = _fixture.Requests
            .Where(r => r.Type == RequestType.Sale &&
                       r.CreatedDate >= startDate &&
                       r.CreatedDate <= endDate)
            .Select(r => r.Client!)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(expectedCount, sellers.Count);
        Assert.Equal(expectedFirstSeller, sellers[0].FullName);
        Assert.Equal(expectedSecondSeller, sellers[1].FullName);
    }

    /// <summary>
    /// Тест 2: Вывести топ 5 клиентов по количеству заявок (отдельно на покупку и продажу)
    /// </summary>
    [Fact]
    public void GetTopClientsReturnsCorrectTopLists()
    {
        var expectedTopCount = 5;
        var expectedBuyer = PetrovaFullName;
        var expectedSeller = IvanovFullName;

        var topBuyers = _fixture.Requests
            .Where(r => r.Type == RequestType.Purchase)
            .GroupBy(r => r.Client)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Select(x => x.Client!)
            .ToList();

        var topSellers = _fixture.Requests
            .Where(r => r.Type == RequestType.Sale)
            .GroupBy(r => r.Client)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Select(x => x.Client!)
            .ToList();

        var buyerNames = topBuyers.Select(c => c.FullName).ToList();
        var sellerNames = topSellers.Select(c => c.FullName).ToList();

        Assert.Equal(expectedTopCount, topBuyers.Count);
        Assert.Equal(expectedTopCount, topSellers.Count);
        Assert.Contains(expectedBuyer, buyerNames);
        Assert.Contains(expectedSeller, sellerNames);
    }

    /// <summary>
    /// Тест 3: Вывести информацию о количестве заявок по каждому типу недвижимости
    /// </summary>
    [Fact]
    public void GetRequestsByPropertyTypeReturnsCorrectCounts()
    {
        var expectedPropertyTypesCount = 5;

        var requestsByType = _fixture.Requests
            .Where(r => r.Property != null) 
            .GroupBy(r => r.Property!.Type)
            .Select(g => new { PropertyType = g.Key, Count = g.Count() })
            .ToDictionary(x => x.PropertyType, x => x.Count);

        var actualApartmentCount = _fixture.Requests.Count(r => r.Property != null && r.Property.Type == PropertyType.Apartment);
        var actualHouseCount = _fixture.Requests.Count(r => r.Property != null && r.Property.Type == PropertyType.House);
        var actualCommercialCount = _fixture.Requests.Count(r => r.Property != null && r.Property.Type == PropertyType.Commercial);
        var actualLandCount = _fixture.Requests.Count(r => r.Property != null && r.Property.Type == PropertyType.Land);
        var actualVillaCount = _fixture.Requests.Count(r => r.Property != null && r.Property.Type == PropertyType.Villa);

        Assert.Equal(expectedPropertyTypesCount, requestsByType.Count);
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
        var expectedClientsCount = 2;
        var expectedFirstClient = PetrovaFullName;
        var expectedSecondClient = FedorovFullName;

        var minPurchaseAmount = _fixture.Requests
            .Where(r => r.Type == RequestType.Purchase)
            .Min(r => r.Amount);

        var minSaleAmount = _fixture.Requests
            .Where(r => r.Type == RequestType.Sale)
            .Min(r => r.Amount);

        var clients = _fixture.Requests
            .Where(r => (r.Type == RequestType.Purchase && r.Amount == minPurchaseAmount) ||
                       (r.Type == RequestType.Sale && r.Amount == minSaleAmount))
            .Select(r => r.Client!)
            .Distinct()
            .ToList();

        Assert.Equal(expectedClientsCount, clients.Count);
        Assert.Contains(clients, c => c.FullName == expectedFirstClient);
        Assert.Contains(clients, c => c.FullName == expectedSecondClient);
    }

    /// <summary>
    /// Тест 5: Вывести сведения о всех клиентах, ищущих недвижимость заданного типа, упорядочить по ФИО
    /// </summary>
    [Fact]
    public void GetClientsByPropertyTypeReturnsSortedClients()
    {
        var propertyType = PropertyType.Apartment;
        var expectedClientsCount = 3;
        var expectedFirstClient = KozlovaFullName;
        var expectedSecondClient = PetrovaFullName;
        var expectedThirdClient = SidorovFullName;

        var clients = _fixture.Requests
            .Where(r => r.Type == RequestType.Purchase &&
                       r.Property != null && 
                       r.Client != null && 
                       r.Property.Type == propertyType)
            .Select(r => r.Client!)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(expectedClientsCount, clients.Count);
        Assert.Equal(expectedFirstClient, clients[0].FullName);
        Assert.Equal(expectedSecondClient, clients[1].FullName);
        Assert.Equal(expectedThirdClient, clients[2].FullName);
    }
}