using EstateAgency.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgency.Domain;

/// <summary>
/// Класс, представляющий заявку от клиента
/// Служит в качестве контракта между клиентом и агентством
/// </summary>
public class Request
{
    /// <summary>
    /// Уникальный идентификатор заявки
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента, создавшего заявку
    /// </summary>
    [Required(ErrorMessage = "Клиент обязателен")]
    public int ClientId { get; set; }

    /// <summary>
    /// Навигационное свойство для клиента
    /// </summary>
    public Client? Client { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    [Required(ErrorMessage = "Объект недвижимости обязателен")]
    public int PropertyId { get; set; }

    /// <summary>
    /// Навигационное свойство для объекта недвижимости
    /// </summary>
    public Property? Property { get; set; }

    /// <summary>
    /// Тип заявки 
    /// </summary>
    [Required(ErrorMessage = "Тип заявки обязателен")]
    public RequestType Type { get; set; }

    /// <summary>
    /// Денежная сумма по заявке
    /// </summary>
    [Required(ErrorMessage = "Сумма заявки обязательна")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата и время создания заявки
    /// </summary>
    [Required(ErrorMessage = "Дата заявки обязательна")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
