using EstateAgency.Domain.Enum;

namespace EstateAgency.Domain.Data;

/// <summary>
/// Класс для заполнения базы данных тестовыми данными
/// </summary>
public static class DataSeeder
{
    private static int _clientId = 1;
    private static int _propertyId = 1;
    private static int _requestId = 1;

    /// <summary>
    /// Заполняет базу данных тестовыми клиентами
    /// </summary>
    private static List<Client> GetTestClients()
    {
        return
        [
            new()
            {
                Id = _clientId++,
                FullName = "Иванов Иван Иванович",
                PassportNumber = "4501 123456",
                PhoneNumber = "+7-999-123-45-67"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Петрова Анна Сергеевна",
                PassportNumber = "4502 234567",
                PhoneNumber = "+7-999-234-56-78"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Сидоров Алексей Петрович",
                PassportNumber = "4503 345678",
                PhoneNumber = "+7-999-345-67-89"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Козлова Мария Владимировна",
                PassportNumber = "4504 456789",
                PhoneNumber = "+7-999-456-78-90"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Федоров Дмитрий Николаевич",
                PassportNumber = "4505 567890",
                PhoneNumber = "+7-999-567-89-01"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Николаев Сергей Викторович",
                PassportNumber = "4506 678901",
                PhoneNumber = "+7-999-678-90-12"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Орлова Екатерина Дмитриевна",
                PassportNumber = "4507 789012",
                PhoneNumber = "+7-999-789-01-23"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Громов Андрей Александрович",
                PassportNumber = "4508 890123",
                PhoneNumber = "+7-999-890-12-34"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Васнецова Ольга Игоревна",
                PassportNumber = "4509 901234",
                PhoneNumber = "+7-999-901-23-45"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Жуковский Павел Сергеевич",
                PassportNumber = "4510 012345",
                PhoneNumber = "+7-999-012-34-56"
            },
            new()
            {
                Id = _clientId++,
                FullName = "Белова Татьяна Михайловна",
                PassportNumber = "4511 112233",
                PhoneNumber = "+7-999-112-23-34"
            }
        ];
    }

    /// <summary>
    /// Заполняет базу данных тестовыми объектами недвижимости
    /// </summary>
    private static List<Property> GetTestProperties()
    {
        return
        [
            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:01:0001001:101",
                Address = "г. Москва, ул. Тверская, д. 1, кв. 10",
                Floors = 9,
                TotalArea = 45.5m,
                Rooms = 2,
                CeilingHeight = 2.75m,
                Floor = 3,
                HasEncumbrances = false
            },
            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:01:0001001:102",
                Address = "г. Москва, ул. Тверская, д. 1, кв. 15",
                Floors = 9,
                TotalArea = 60.3m,
                Rooms = 3,
                CeilingHeight = 2.75m,
                Floor = 5,
                HasEncumbrances = false
            },
            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:01:0001001:103",
                Address = "г. Москва, ул. Арбат, д. 25, кв. 7",
                Floors = 5,
                TotalArea = 35.2m,
                Rooms = 1,
                CeilingHeight = 2.6m,
                Floor = 2,
                HasEncumbrances = true
            },
            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:01:0001001:104",
                Address = "г. Москва, ул. Новый Арбат, д. 15, кв. 22",
                Floors = 12,
                TotalArea = 85.0m,
                Rooms = 4,
                CeilingHeight = 3.2m,
                Floor = 8,
                HasEncumbrances = false
            },

            new()
            {
                Id = _propertyId++,
                Type = PropertyType.House,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:02:0002001:201",
                Address = "МО, г. Красногорск, ул. Центральная, д. 25",
                Floors = 2,
                TotalArea = 120.0m,
                Rooms = 5,
                CeilingHeight = 3.0m,
                Floor = 1,
                HasEncumbrances = false
            },
            new()
            {
                Id = _propertyId++,
                Type = PropertyType.House,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:02:0002001:202",
                Address = "МО, г. Одинцово, ул. Садовая, д. 15",
                Floors = 3,
                TotalArea = 180.5m,
                Rooms = 7,
                CeilingHeight = 3.2m,
                Floor = 1,
                HasEncumbrances = true
            },
            new()
            {
                Id = _propertyId++,
                Type = PropertyType.House,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:02:0002001:203",
                Address = "МО, г. Химки, ул. Ленина, д. 42",
                Floors = 2,
                TotalArea = 95.0m,
                Rooms = 4,
                CeilingHeight = 2.8m,
                Floor = 1,
                HasEncumbrances = false
            },

            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Commercial,
                Purpose = PropertyPurpose.Commercial,
                CadastralNumber = "77:03:0003001:301",
                Address = "г. Москва, ул. Новый Арбат, д. 15, офис 100",
                Floors = 12,
                TotalArea = 85.0m,
                Rooms = 3,
                CeilingHeight = 3.2m,
                Floor = 10,
                HasEncumbrances = false
            },
            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Commercial,
                Purpose = PropertyPurpose.Commercial,
                CadastralNumber = "77:03:0003001:302",
                Address = "г. Москва, Цветной бульвар, д. 2, офис 305",
                Floors = 8,
                TotalArea = 120.0m,
                Rooms = 5,
                CeilingHeight = 3.5m,
                Floor = 3,
                HasEncumbrances = true
            },

            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Land,
                Purpose = PropertyPurpose.Agricultural,
                CadastralNumber = "77:04:0004001:401",
                Address = "МО, Ленинский р-н, д. Петрово, уч. 15",
                Floors = 0,
                TotalArea = 800.0m,
                Rooms = 0,
                CeilingHeight = null,
                Floor = null,
                HasEncumbrances = true
            },

            new()
            {
                Id = _propertyId++,
                Type = PropertyType.Villa,
                Purpose = PropertyPurpose.Residential,
                CadastralNumber = "77:05:0005001:501",
                Address = "МО, Рублево-Успенское шоссе, коттеджный поселок",
                Floors = 3,
                TotalArea = 350.0m,
                Rooms = 8,
                CeilingHeight = 3.5m,
                Floor = 1,
                HasEncumbrances = false
            }
        ];
    }

    /// <summary>
    /// Заполняет базу данных тестовыми заявками
    /// </summary>
    private static List<Request> GetTestRequests(List<Client> clients, List<Property> properties)
    {
        return
        [
            new()
            {
                Id = _requestId++,
                ClientId = clients[0].Id,
                Client = clients[0],
                PropertyId = properties[0].Id,
                Property = properties[0],
                Type = RequestType.Sale,
                Amount = 8500000m,
                CreatedDate = new DateTime(2024, 1, 15)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[1].Id,
                Client = clients[1],
                PropertyId = properties[0].Id,
                Property = properties[0],
                Type = RequestType.Purchase,
                Amount = 8200000m,
                CreatedDate = new DateTime(2024, 1, 20)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[2].Id,
                Client = clients[2],
                PropertyId = properties[1].Id,
                Property = properties[1],
                Type = RequestType.Sale,
                Amount = 12000000m,
                CreatedDate = new DateTime(2024, 2, 10)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[0].Id,
                Client = clients[0],
                PropertyId = properties[4].Id,
                Property = properties[4],
                Type = RequestType.Sale,
                Amount = 18500000m,
                CreatedDate = new DateTime(2024, 2, 15)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[3].Id,
                Client = clients[3],
                PropertyId = properties[1].Id,
                Property = properties[1],
                Type = RequestType.Purchase,
                Amount = 11500000m,
                CreatedDate = new DateTime(2024, 2, 20)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[4].Id,
                Client = clients[4],
                PropertyId = properties[9].Id,
                Property = properties[9],
                Type = RequestType.Sale,
                Amount = 5000000m,
                CreatedDate = new DateTime(2024, 3, 1)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[1].Id,
                Client = clients[1],
                PropertyId = properties[4].Id,
                Property = properties[4],
                Type = RequestType.Purchase,
                Amount = 18000000m,
                CreatedDate = new DateTime(2024, 3, 5)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[2].Id,
                Client = clients[2],
                PropertyId = properties[0].Id,
                Property = properties[0],
                Type = RequestType.Purchase,
                Amount = 8300000m,
                CreatedDate = new DateTime(2024, 3, 10)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[3].Id,
                Client = clients[3],
                PropertyId = properties[7].Id,
                Property = properties[7],
                Type = RequestType.Sale,
                Amount = 25000000m,
                CreatedDate = new DateTime(2024, 3, 15)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[4].Id,
                Client = clients[4],
                PropertyId = properties[7].Id,
                Property = properties[7],
                Type = RequestType.Purchase,
                Amount = 24500000m,
                CreatedDate = new DateTime(2024, 3, 20)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[5].Id,
                Client = clients[5],
                PropertyId = properties[2].Id,
                Property = properties[2],
                Type = RequestType.Sale,
                Amount = 7500000m,
                CreatedDate = new DateTime(2024, 4, 1)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[6].Id,
                Client = clients[6],
                PropertyId = properties[10].Id,
                Property = properties[10],
                Type = RequestType.Sale,
                Amount = 50000000m,
                CreatedDate = new DateTime(2024, 4, 5)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[7].Id,
                Client = clients[7],
                PropertyId = properties[5].Id,
                Property = properties[5],
                Type = RequestType.Sale,
                Amount = 15000000m,
                CreatedDate = new DateTime(2024, 4, 10)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[8].Id,
                Client = clients[8],
                PropertyId = properties[6].Id,
                Property = properties[6],
                Type = RequestType.Purchase,
                Amount = 9500000m,
                CreatedDate = new DateTime(2024, 4, 15)
            }
        ];
    }

    /// <summary>
    /// Создает полный набор тестовых данных (клиенты, недвижимость, заявки)
    /// </summary>
    public static (List<Client> clients, List<Property> properties, List<Request> requests) GetCompleteTestData()
    {
        var clients = GetTestClients();
        var properties = GetTestProperties();
        var requests = GetTestRequests(clients, properties);

        foreach (var request in requests)
        {
            request.Client.Requests.Add(request);
            request.Property.Requests.Add(request);
        }

        return (clients, properties, requests);
    }
}
