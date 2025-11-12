using System.ComponentModel.DataAnnotations;
using EstateAgency.Domain.Enum;

namespace EstateAgency.Application.DTOs.Requests;

/// <summary> 
/// Содержит валидационные правила для обновления данных заявки 
/// </summary>
public class RequestUpdateDto
{
    /// <summary>
    /// Идентификатор клиента 
    /// </summary>
    [Required(ErrorMessage = "Идентификатор клиента обязателен")]
    public int? ClientId { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости 
    /// </summary>
    [Required(ErrorMessage = "Идентификатор недвижимости обязателен")]
    public int? PropertyId { get; set; }

    /// <summary>
    /// Тип заявки 
    /// </summary>
    [Required(ErrorMessage = "Тип заявки обязателен")]
    public RequestType? Type { get; set; }

    /// <summary>
    /// Денежная сумма заявки 
    /// </summary>
    [Required(ErrorMessage = "Сумма заявки обязательна")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
    public decimal? Amount { get; set; }
}