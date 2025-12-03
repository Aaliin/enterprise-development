namespace EstateAgency.Application.Dto;

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
    public required string FullName { get; set; }

    /// <summary>
    /// Номер паспорта клиента в формате "XXXX XXXXXX"
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Контактный телефон клиента в международном формате
    /// </summary>
    public required string PhoneNumber { get; set; }
}