using EstateAgency.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Application.Dto;

/// <summary> 
/// Содержит валидационные правила для финансовых данных и связей 
/// </summary>
public class CreateRequestDto
{
    /// <summary>
    /// Идентификатор клиента, создающего заявку 
    /// </summary>
    [Required(ErrorMessage = "Идентификатор клиента обязателен")]
    public int ClientId { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости, к которому относится заявка 
    /// </summary>
    [Required(ErrorMessage = "Идентификатор недвижимости обязателен")]
    public int PropertyId { get; set; }

    /// <summary>
    /// Тип заявки 
    /// </summary>
    [Required(ErrorMessage = "Тип заявки обязателен")]
    public RequestType Type { get; set; }

    /// <summary>
    /// Денежная сумма заявки
    /// </summary>
    [Required(ErrorMessage = "Сумма заявки обязательна")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
    public decimal Amount { get; set; }
}