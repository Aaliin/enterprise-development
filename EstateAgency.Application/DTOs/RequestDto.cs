using EstateAgency.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Application.DTOs;

/// <summary> 
/// Содержит полные данные заявки для отображения в системе 
/// </summary>
public class RequestDto
{
    /// <summary>
    /// Уникальный идентификатор заявки в системе
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента  
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Полное имя клиента
    /// </summary>
    public string ClientFullName { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор объекта недвижимости 
    /// </summary>
    public int PropertyId { get; set; }

    /// <summary>
    /// Адрес объекта недвижимости
    /// </summary>
    public string PropertyAddress { get; set; } = string.Empty;

    /// <summary>
    /// Тип заявки  
    /// </summary>
    public RequestType Type { get; set; }

    /// <summary>
    /// Денежная сумма заявки 
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата и время создания заявки 
    /// </summary>
    public DateTime CreatedDate { get; set; }
}

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