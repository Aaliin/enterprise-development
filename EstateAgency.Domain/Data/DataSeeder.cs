using EstateAgency.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public static List<Client> GetTestClients()
    {
        return new List<Client>
        {
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
        };
    }

    /// <summary>
    /// Заполняет базу данных тестовыми объектами недвижимости
    /// </summary>
    public static List<Property> GetTestProperties()
    {
        return new List<Property>
        {
            // Квартиры (4 экземпляра)
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

            // Дома (3 экземпляра)
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

            // Коммерческая недвижимость (2 экземпляра)
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

            // Земельные участки (1 экземпляр)
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

            // Виллы (1 экземпляр)
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
        };
    }

    /// <summary>
    /// Заполняет базу данных тестовыми заявками
    /// </summary>
    public static List<Request> GetTestRequests(List<Client> clients, List<Property> properties)
    {
        return new List<Request>
        {
            // Заявки на январь 2024 
            new()
            {
                Id = _requestId++,
                ClientId = clients[0].Id,
                PropertyId = properties[0].Id,
                Type = RequestType.Sale,
                Amount = 8500000m,
                CreatedDate = new DateTime(2024, 1, 15)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[1].Id,
                PropertyId = properties[0].Id,
                Type = RequestType.Purchase,
                Amount = 8200000m,
                CreatedDate = new DateTime(2024, 1, 20)
            },

            // Заявки на февраль 2024
            new()
            {
                Id = _requestId++,
                ClientId = clients[2].Id,
                PropertyId = properties[1].Id,
                Type = RequestType.Sale,
                Amount = 12000000m,
                CreatedDate = new DateTime(2024, 2, 10)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[0].Id,
                PropertyId = properties[4].Id,
                Type = RequestType.Sale,
                Amount = 18500000m,
                CreatedDate = new DateTime(2024, 2, 15)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[3].Id,
                PropertyId = properties[1].Id,
                Type = RequestType.Purchase,
                Amount = 11500000m,
                CreatedDate = new DateTime(2024, 2, 20)
            },

            // Заявки на март 2024
            new()
            {
                Id = _requestId++,
                ClientId = clients[4].Id,
                PropertyId = properties[9].Id,
                Type = RequestType.Sale,
                Amount = 5000000m,
                CreatedDate = new DateTime(2024, 3, 1)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[1].Id,
                PropertyId = properties[4].Id,
                Type = RequestType.Purchase,
                Amount = 18000000m,
                CreatedDate = new DateTime(2024, 3, 5)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[2].Id,
                PropertyId = properties[0].Id,
                Type = RequestType.Purchase,
                Amount = 8300000m,
                CreatedDate = new DateTime(2024, 3, 10)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[3].Id,
                PropertyId = properties[7].Id,
                Type = RequestType.Sale,
                Amount = 25000000m,
                CreatedDate = new DateTime(2024, 3, 15)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[4].Id,
                PropertyId = properties[7].Id,
                Type = RequestType.Purchase,
                Amount = 24500000m,
                CreatedDate = new DateTime(2024, 3, 20)
            },

            // Заявки на апрель 2024
            new()
            {
                Id = _requestId++,
                ClientId = clients[5].Id,
                PropertyId = properties[2].Id,
                Type = RequestType.Sale,
                Amount = 7500000m,
                CreatedDate = new DateTime(2024, 4, 1)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[6].Id,
                PropertyId = properties[10].Id,
                Type = RequestType.Sale,
                Amount = 50000000m,
                CreatedDate = new DateTime(2024, 4, 5)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[7].Id,
                PropertyId = properties[5].Id,
                Type = RequestType.Sale,
                Amount = 15000000m,
                CreatedDate = new DateTime(2024, 4, 10)
            },
            new()
            {
                Id = _requestId++,
                ClientId = clients[8].Id,
                PropertyId = properties[6].Id,
                Type = RequestType.Purchase,
                Amount = 9500000m,
                CreatedDate = new DateTime(2024, 4, 15)
            }
        };
    }

    /// <summary>
    /// Создает полный набор тестовых данных (клиенты, недвижимость, заявки)
    /// </summary>
    public static (List<Client> clients, List<Property> properties, List<Request> requests) GetCompleteTestData()
    {
        var clients = GetTestClients();
        var properties = GetTestProperties();
        var requests = GetTestRequests(clients, properties);

        return (clients, properties, requests);
    }

    /// <summary>
    /// Заполняет DbContext тестовыми данными
    /// </summary>
    public static void SeedDatabase(Microsoft.EntityFrameworkCore.DbContext context)
    {
        if (context.Set<Client>().Any())
            return;

        var (clients, properties, requests) = GetCompleteTestData();

        context.Set<Client>().AddRange(clients);
        context.Set<Property>().AddRange(properties);
        context.SaveChanges(); 

        context.Set<Request>().AddRange(requests);
        context.SaveChanges();
    }
}
