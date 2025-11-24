using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.EF.Data;

/// <summary>
/// Класс для заполнения базы данных тестовыми данными
/// </summary>
public class DataSeeder(EstateAgencyDbContext context)
{
    /// <summary>
    /// Заполняет базу данных тестовыми данными
    /// </summary>
    public async Task SeedAsync()
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await SeedClientsAsync();
        await SeedPropertiesAsync();
        await SeedRequestsAsync();
    }

    /// <summary>
    /// Заполняет базу данных тестовыми клиентами
    /// </summary>
    private async Task SeedClientsAsync()
    {
        var clients = new List<Client>
        {
            new()
            {
                FullName = "Иванов Иван Иванович",
                PassportNumber = "4501 123456",
                PhoneNumber = "+7-999-123-45-67"
            },
            new()
            {
                FullName = "Петрова Анна Сергеевна",
                PassportNumber = "4502 234567",
                PhoneNumber = "+7-999-234-56-78"
            },
            new()
            {
                FullName = "Сидоров Алексей Петрович",
                PassportNumber = "4503 345678",
                PhoneNumber = "+7-999-345-67-89"
            },
            new()
            {
                FullName = "Козлова Мария Владимировна",
                PassportNumber = "4504 456789",
                PhoneNumber = "+7-999-456-78-90"
            },
            new()
            {
                FullName = "Федоров Дмитрий Николаевич",
                PassportNumber = "4505 567890",
                PhoneNumber = "+7-999-567-89-01"
            },
            new()
            {
                FullName = "Николаев Сергей Викторович",
                PassportNumber = "4506 678901",
                PhoneNumber = "+7-999-678-90-12"
            },
            new()
            {
                FullName = "Орлова Екатерина Дмитриевна",
                PassportNumber = "4507 789012",
                PhoneNumber = "+7-999-789-01-23"
            },
            new()
            {
                FullName = "Громов Андрей Александрович",
                PassportNumber = "4508 890123",
                PhoneNumber = "+7-999-890-12-34"
            },
            new()
            {
                FullName = "Васнецова Ольга Игоревна",
                PassportNumber = "4509 901234",
                PhoneNumber = "+7-999-901-23-45"
            },
            new()
            {
                FullName = "Жуковский Павел Сергеевич",
                PassportNumber = "4510 012345",
                PhoneNumber = "+7-999-012-34-56"
            },
            new()
            {
                FullName = "Белова Татьяна Михайловна",
                PassportNumber = "4511 112233",
                PhoneNumber = "+7-999-112-23-34"
            }
        };

        await context.Clients.AddRangeAsync(clients);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Заполняет базу данных тестовыми объектами недвижимости
    /// </summary>
    private async Task SeedPropertiesAsync()
    {
        var properties = new List<Property>
        {
            new()
            {
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

        await context.Properties.AddRangeAsync(properties);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Заполняет базу данных тестовыми заявками
    /// </summary>
    private async Task SeedRequestsAsync()
    {
        var clients = await context.Clients.ToListAsync();
        var properties = await context.Properties.ToListAsync();

        var requests = new List<Request>
    {
        new()
        {
            ClientId = clients[0].Id,
            PropertyId = properties[0].Id,
            Type = RequestType.Sale,
            Amount = 8500000m,
            CreatedDate = new DateTime(2024, 1, 15)
        },
        new()
        {
            ClientId = clients[1].Id,
            PropertyId = properties[0].Id,
            Type = RequestType.Purchase,
            Amount = 8200000m,
            CreatedDate = new DateTime(2024, 1, 20)
        },
        new()
        {
            ClientId = clients[2].Id,
            PropertyId = properties[1].Id,
            Type = RequestType.Sale,
            Amount = 12000000m,
            CreatedDate = new DateTime(2024, 2, 10)
        },
        new()
        {
            ClientId = clients[0].Id,
            PropertyId = properties[4].Id,
            Type = RequestType.Sale,
            Amount = 18500000m,
            CreatedDate = new DateTime(2024, 2, 15)
        },
        new()
        {
            ClientId = clients[3].Id,
            PropertyId = properties[1].Id,
            Type = RequestType.Purchase,
            Amount = 11500000m,
            CreatedDate = new DateTime(2024, 2, 20)
        },
        new()
        {
            ClientId = clients[4].Id,
            PropertyId = properties[9].Id,
            Type = RequestType.Sale,
            Amount = 5000000m,
            CreatedDate = new DateTime(2024, 3, 1)
        },
        new()
        {
            ClientId = clients[1].Id,
            PropertyId = properties[4].Id,
            Type = RequestType.Purchase,
            Amount = 18000000m,
            CreatedDate = new DateTime(2024, 3, 5)
        },
        new()
        {
            ClientId = clients[2].Id,
            PropertyId = properties[0].Id,
            Type = RequestType.Purchase,
            Amount = 8300000m,
            CreatedDate = new DateTime(2024, 3, 10)
        },
        new()
        {
            ClientId = clients[3].Id,
            PropertyId = properties[7].Id,
            Type = RequestType.Sale,
            Amount = 25000000m,
            CreatedDate = new DateTime(2024, 3, 15)
        },
        new()
        {
            ClientId = clients[4].Id,
            PropertyId = properties[7].Id,
            Type = RequestType.Purchase,
            Amount = 24500000m,
            CreatedDate = new DateTime(2024, 3, 20)
        },
        new()
        {
            ClientId = clients[5].Id,
            PropertyId = properties[2].Id,
            Type = RequestType.Sale,
            Amount = 7500000m,
            CreatedDate = new DateTime(2024, 4, 1)
        },
        new()
        {
            ClientId = clients[6].Id,
            PropertyId = properties[10].Id,
            Type = RequestType.Sale,
            Amount = 50000000m,
            CreatedDate = new DateTime(2024, 4, 5)
        },
        new()
        {
            ClientId = clients[7].Id,
            PropertyId = properties[5].Id,
            Type = RequestType.Sale,
            Amount = 15000000m,
            CreatedDate = new DateTime(2024, 4, 10)
        },
        new()
        {
            ClientId = clients[8].Id,
            PropertyId = properties[6].Id,
            Type = RequestType.Purchase,
            Amount = 9500000m,
            CreatedDate = new DateTime(2024, 4, 15)
        }
    };

        await context.Requests.AddRangeAsync(requests);
        await context.SaveChangesAsync();
    }
}