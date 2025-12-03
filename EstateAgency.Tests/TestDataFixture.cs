using EstateAgency.Domain.Entities;
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
        var (clients, properties) = SampleData.GetCompleteTestData();

        for (var i = 0; i < clients.Count; i++)
        {
            clients[i].Id = i + 1;
        }

        for (var i = 0; i < properties.Count; i++)
        {
            properties[i].Id = i + 1;
        }

        var requests = SampleData.CreateSampleRequests(clients, properties);

        for (var i = 0; i < requests.Count; i++)
        {
            var request = requests[i];
            request.Id = i + 1;

            var client = clients.FirstOrDefault(c => c.Id == request.ClientId);
            if (client != null)
            {
                request.Client = client;
            }

            var property = properties.FirstOrDefault(p => p.Id == request.PropertyId);
            if (property != null)
            {
                request.Property = property;
            }
        }

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