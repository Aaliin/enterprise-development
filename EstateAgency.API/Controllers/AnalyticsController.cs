using EstateAgency.Application.DTOs.Analytics;
using EstateAgency.Application.Interfaces.Services;
using EstateAgency.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.API.Controllers;

/// <summary>
/// Контроллер для работы с аналитикой и отчетами риэлторского агентства
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    private readonly IAnalyticsService _analyticsService = analyticsService;

    /// <summary>
    /// Получает статистику по продавцам за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param>
    [HttpGet("sellers-by-period")]
    public async Task<ActionResult<List<SellerPeriodDto>>> GetSellersByPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (startDate >= endDate)
            return BadRequest("Start date must be before end date");

        var result = await _analyticsService.GetSellersByPeriodAsync(startDate, endDate);
        return Ok(result);
    }

    /// <summary>
    /// Получает топ клиентов по количеству заявок указанного типа
    /// </summary>
    /// <param name="type">Тип заявки (покупка/продажа)</param>
    /// <param name="topCount">Количество клиентов в топе (по умолчанию 5)</param>
    [HttpGet("top-clients-by-requests")]
    public async Task<ActionResult<List<ClientAnalyticsDto>>> GetTopClientsByRequests(
        [FromQuery] RequestType type,
        [FromQuery] int topCount = 5)
    {
        var result = await _analyticsService.GetTopClientsByRequestsAsync(type, topCount);
        return Ok(result);
    }

    /// <summary>
    /// Получает статистику количества заявок по типам недвижимости
    /// </summary>
    [HttpGet("requests-by-property-type")]
    public async Task<ActionResult<List<PropertyTypeStatDto>>> GetRequestCountByPropertyType()
    {
        var result = await _analyticsService.GetRequestCountByPropertyTypeAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получает клиентов с заявками минимальной стоимости
    /// </summary>
    [HttpGet("clients-min-amount")]
    public async Task<ActionResult<List<ClientAnalyticsDto>>> GetClientsWithMinAmountRequest()
    {
        var result = await _analyticsService.GetClientsWithMinAmountRequestAsync();
        return Ok(result);
    }

    /// <summary>
    /// Получает клиентов, ищущих недвижимость заданного типа
    /// </summary>
    /// <param name="propertyType">Тип недвижимости для поиска</param>
    [HttpGet("clients-by-property-type")]
    public async Task<ActionResult<List<ClientAnalyticsDto>>> GetClientsSearchingPropertyType(
        [FromQuery] PropertyType propertyType)
    {
        var result = await _analyticsService.GetClientsSearchingPropertyTypeAsync(propertyType);
        return Ok(result);
    }
}