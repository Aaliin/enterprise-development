using EstateAgency.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Domain.Entities;

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
    public required int ClientId { get; set; }

    /// <summary>
    /// Навигационное свойство для клиента
    /// </summary>
    public Client? Client { get; set; }

    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    [Required(ErrorMessage = "Объект недвижимости обязателен")]
    public required int PropertyId { get; set; }

    /// <summary>
    /// Навигационное свойство для объекта недвижимости
    /// </summary>
    public Property? Property { get; set; }

    /// <summary>
    /// Тип заявки 
    /// </summary>
    [Required(ErrorMessage = "Тип заявки обязателен")]
    public required RequestType Type { get; set; }

    /// <summary>
    /// Денежная сумма по заявке
    /// </summary>
    [Required(ErrorMessage = "Сумма заявки обязательна")]
    public required decimal Amount { get; set; }

    /// <summary>
    /// Дата и время создания заявки
    /// </summary>
    [Required(ErrorMessage = "Дата заявки обязательна")]
    public required DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
