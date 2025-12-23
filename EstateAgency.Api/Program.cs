using EstateAgency.Api.Services;
using EstateAgency.Application.Interfaces;
using EstateAgency.Application.Mappings;
using EstateAgency.Application.Services;
using EstateAgency.ContractGenerator.Models;
using EstateAgency.ContractGenerator.Services;
using EstateAgency.Domain.Data;
using EstateAgency.Domain.Interfaces;
using EstateAgency.Ef.Data;
using EstateAgency.Ef.Repositories;
using EstateAgency.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NATS.Client.Core;

var builder = WebApplication.CreateBuilder(args);

var appMode = Environment.GetEnvironmentVariable("AppMode") ?? builder.Configuration["AppMode"] ?? "Lab4";
Console.WriteLine($"=== Operating Mode: {appMode} ===");

var useEf = appMode == "Lab3" || appMode == "Lab4";
var useNats = appMode == "Lab4";

if (useEf)
{
    Console.WriteLine("Using Entity Framework + SQL Server");
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    Console.WriteLine($"Database: {connectionString}");

    builder.Services.AddDbContext<EstateAgencyDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddScoped<IClientRepository, EfClientRepository>();
    builder.Services.AddScoped<IPropertyRepository, EfPropertyRepository>();
    builder.Services.AddScoped<IRequestRepository, EfRequestRepository>();
}
else
{
    Console.WriteLine("Using InMemory repositories");

    builder.Services.AddScoped<IClientRepository, InMemoryClientRepository>();
    builder.Services.AddScoped<IPropertyRepository, InMemoryPropertyRepository>();
    builder.Services.AddScoped<IRequestRepository, InMemoryRequestRepository>();
}

if (useNats)
{
    Console.WriteLine("Configuring NATS integration");

    var natsConfigSection = builder.Configuration.GetSection("Nats");
    var natsUrl = natsConfigSection["Url"] ?? "nats://localhost:4222";

    builder.Services.AddSingleton<INatsConnection>(serviceProvider =>
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        var opts = new NatsOpts
        {
            Url = natsUrl,
            ConnectTimeout = TimeSpan.FromSeconds(10),
            ReconnectWaitMax = TimeSpan.FromSeconds(5),
            Name = "EstateAgency.Api",
            Echo = false
        };

        var connection = new NatsConnection(opts);

        return new NatsConnection(opts);
    });

    builder.Services.AddHostedService<NatsConsumerService>();
    builder.Services.AddScoped<INatsPublisher, NatsPublisher>();
    builder.Services.Configure<NatsOptions>(builder.Configuration.GetSection("Nats"));
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRequestService, RequestService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if (useEf)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<EstateAgencyDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        logger.LogInformation("Database created");

        var (clients, properties) = SampleData.GetCompleteTestData();

        await context.Clients.AddRangeAsync(clients);
        await context.SaveChangesAsync();
        var savedClients = await context.Clients.ToListAsync();
        logger.LogInformation("Saved clients: {Count}", savedClients.Count);

        await context.Properties.AddRangeAsync(properties);
        await context.SaveChangesAsync();
        var savedProperties = await context.Properties.ToListAsync();
        logger.LogInformation("Saved properties: {Count}", savedProperties.Count);

        var requests = SampleData.CreateSampleRequests(savedClients, savedProperties);
        await context.Requests.AddRangeAsync(requests);
        await context.SaveChangesAsync();
        logger.LogInformation("Saved requests: {Count}", requests.Count);

        if (useNats)
        {
            logger.LogInformation("NATS Consumer Service ready to receive messages");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database initialization error");
        throw;
    }
}

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    mode = appMode,
    nats = useNats ? "enabled" : "disabled"
}));
app.MapGet("/", () => $"Estate Agency API running in {appMode} mode. Swagger available at /swagger");

app.Run();