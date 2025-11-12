using EstateAgency.Application.Interfaces.Services;
using EstateAgency.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EstateAgency.Application;

/// <summary>
/// Осуществляет конфигурацию зависимостей для всего слоя приложения
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует все сервисы, мапперы и валидаторы Application слоя
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации</param>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();

        return services;
    }
}