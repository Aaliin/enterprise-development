namespace EstateAgency.Application.DTOs.Clients;

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