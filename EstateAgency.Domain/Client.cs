using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Domain;

/// <summary>
/// Класс, представляющий клиента 
/// </summary>
public class Client
{
    /// <summary>
    /// Уникальный идентификатор клиента
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя клиента (ФИО)
    /// </summary>
    [Required(ErrorMessage = "ФИО клиента обязательно")]
    public required string FullName { get; set; }

    /// <summary>
    /// Номер паспорта клиента
    /// </summary>
    [Required(ErrorMessage = "Номер паспорта обязателен")]
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Контактный телефон клиента
    /// </summary>
    [Required(ErrorMessage = "Контактный телефон обязателен")]
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Коллекция заявок, созданных данным клиентом
    /// </summary>
    public List<Request> Requests { get; set; } = [];
}
