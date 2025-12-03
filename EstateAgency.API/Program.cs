using EstateAgency.Application.Interfaces;
using EstateAgency.Application.Services;
using EstateAgency.Application.Mappings;
using EstateAgency.Domain.Interfaces;
using EstateAgency.Domain.Data;
using EstateAgency.Infrastructure.Repositories; 
using EstateAgency.EF.Repositories;            
using EstateAgency.EF.Data;                   
using Microsoft.EntityFrameworkCore;
using EstateAgency.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

var useEf = builder.Configuration.GetValue<bool>("UseEf")
            || builder.Configuration.GetConnectionString("DefaultConnection") != null;

if (useEf)
{
    Console.WriteLine("Running in EF (Lab 3)");

    builder.AddServiceDefaults();

    builder.Services.AddDbContext<EstateAgencyDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IClientRepository, EfClientRepository>();
    builder.Services.AddScoped<IPropertyRepository, EfPropertyRepository>();
    builder.Services.AddScoped<IRequestRepository, EfRequestRepository>();
}
else
{
    Console.WriteLine("Running in InMemory (Lab 2)");

    builder.Services.AddScoped<IClientRepository, InMemoryClientRepository>();
    builder.Services.AddScoped<IPropertyRepository, InMemoryPropertyRepository>();
    builder.Services.AddScoped<IRequestRepository, InMemoryRequestRepository>();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRequestService, RequestService>();

var app = builder.Build();

if (useEf)
{
    app.MapDefaultEndpoints(); 
}

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

    try
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var (clients, properties) = SampleData.GetCompleteTestData();

        await context.Clients.AddRangeAsync(clients);
        await context.SaveChangesAsync();
        var savedClients = await context.Clients.OrderBy(c => c.Id).ToListAsync();
        Console.WriteLine($"Saved {clients.Count} clients");

        await context.Properties.AddRangeAsync(properties);
        await context.SaveChangesAsync();
        var savedProperties = await context.Properties.OrderBy(p => p.Id).ToListAsync();
        Console.WriteLine($"Saved {properties.Count} properties");

        var requests = SampleData.CreateSampleRequests(savedClients, savedProperties);
        await context.Requests.AddRangeAsync(requests);
        await context.SaveChangesAsync();
        Console.WriteLine($"Saved {requests.Count} requests");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.ToString()); 
        throw; 
    }
}

app.Run();