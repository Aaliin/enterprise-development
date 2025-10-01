using EstateAgency.Domain;
using EstateAgency.Domain.Data;

namespace EstateAgency.Tests;

/// <summary>
/// Инициализирует и предоставляет тестовые данные
/// </summary>
public class TestDataFixture : IDisposable
{
    /// <summary>
    /// Список тестовых клиентов
    /// </summary>
    public List<Client> Clients { get; private set; } = [];

    /// <summary>
    /// Список тестовых объектов недвижимости
    /// </summary>
    public List<Property> Properties { get; private set; } = [];

    /// <summary>
    /// Список тестовых заявок
    /// </summary>
    public List<Request> Requests { get; private set; } = [];

    /// <summary>
    /// Инициализирует тестовые данные
    /// </summary>
    public TestDataFixture()
    {
        InitializeTestData();
    }

    /// <summary>
    /// Инициализирует тестовые данные с использованием DataSeeder
    /// </summary>
    private void InitializeTestData()
    {
        var (clients, properties, requests) = DataSeeder.GetCompleteTestData();

        Clients = clients;
        Properties = properties;
        Requests = requests;
    }

    /// <summary>
    /// Очистка ресурсов
    /// </summary>
    public void Dispose()
    {
        Clients.Clear();
        Properties.Clear();
        Requests.Clear();

        GC.SuppressFinalize(this);
    }
}