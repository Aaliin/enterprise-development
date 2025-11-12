using EstateAgency.Application.Interfaces.Repositories;
using EstateAgency.Infrastructure.Data;
using EstateAgency.Infrastructure.Repositories;
using EstateAgency.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructure;

/// <summary>
/// Осуществляет конфигурацию зависимостей для доступа к данным и внешним сервисам
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Используется для разработки, тестирования и демонстрационных целей
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации</param>
    public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("EstateAgencyDb"));

        services.AddScoped<IEstateAgencyRepository, EstateAgencyRepository>();
        services.AddScoped<IDateTimeService, DateTimeService>();

        return services;
    }
}