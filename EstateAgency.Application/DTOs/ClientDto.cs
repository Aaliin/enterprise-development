using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Application.DTOs;

/// <summary>
/// Содержит основные данные клиента и список связанных заявок
/// </summary>
public class ClientDto
{
    /// <summary>
    /// Уникальный идентификатор клиента
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя клиента в формате "Фамилия Имя Отчество"
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Номер паспорта клиента в формате "XXXX XXXXXX"
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Контактный телефон клиента в международном формате
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
}

/// <summary>
/// Содержит валидационные правила для входящих данных
/// </summary>
public class CreateClientDto
{
    /// <summary>
    /// Полное имя клиента. Обязательное поле, максимум 100 символов
    /// </summary>
    [Required(ErrorMessage = "ФИО клиента обязательно")]
    [StringLength(100, ErrorMessage = "ФИО не может превышать 100 символов")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Номер паспорта в формате "XXXX XXXXXX"
    /// </summary>
    [Required(ErrorMessage = "Номер паспорта обязателен")]
    [RegularExpression(@"^\d{4} \d{6}$", ErrorMessage = "Номер паспорта должен быть в формате 'XXXX XXXXXX'")]
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Контактный телефон для связи с клиентом
    /// </summary>
    [Required(ErrorMessage = "Контактный телефон обязателен")]
    [Phone(ErrorMessage = "Некорректный формат телефона")]
    public string PhoneNumber { get; set; } = string.Empty;
}