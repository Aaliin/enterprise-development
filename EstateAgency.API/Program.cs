using EstateAgency.Application.Interfaces;
using EstateAgency.Application.Services;
using EstateAgency.Application.Mappings;
using EstateAgency.Domain.Interfaces;
using EstateAgency.Infrastructure.Repositories; 
using EstateAgency.EF.Repositories;            
using EstateAgency.EF.Data;                   
using Microsoft.EntityFrameworkCore;
using EstateAgency.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

var useEF = builder.Configuration.GetValue<bool>("UseEF")
            || builder.Configuration.GetConnectionString("SqlServerConnection") != null;

if (useEF)
{
    Console.WriteLine("Running in EF (Lab 3)");

    builder.AddServiceDefaults();

    builder.Services.AddDbContext<EstateAgencyDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

    builder.Services.AddScoped<IClientRepository, EfClientRepository>();
    builder.Services.AddScoped<IPropertyRepository, EfPropertyRepository>();
    builder.Services.AddScoped<IRequestRepository, EfRequestRepository>();

    builder.Services.AddScoped<DataSeeder>();
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

if (useEF)
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

if (useEF)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<EstateAgencyDbContext>();
    await context.Database.EnsureCreatedAsync();  

    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.Run();