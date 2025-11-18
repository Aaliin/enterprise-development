using EstateAgency.Application.Interfaces;
using EstateAgency.Application.Services;
using EstateAgency.Application.Mappings;
using EstateAgency.Domain.Interfaces;
using EstateAgency.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IClientRepository, InMemoryClientRepository>();
builder.Services.AddScoped<IPropertyRepository, InMemoryPropertyRepository>();
builder.Services.AddScoped<IRequestRepository, InMemoryRequestRepository>();

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

app.Run();