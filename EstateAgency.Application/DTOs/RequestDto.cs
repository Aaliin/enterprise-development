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
    public required string ClientFullName { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости 
    /// </summary>
    public int PropertyId { get; set; }

    /// <summary>
    /// Адрес объекта недвижимости
    /// </summary>
    public required string PropertyAddress { get; set; } 

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