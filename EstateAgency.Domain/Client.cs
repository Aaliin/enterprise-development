using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Номер паспорта клиента
    /// </summary>
    [Required(ErrorMessage = "Номер паспорта обязателен")]
    public string PassportNumber { get; set; } = string.Empty;

    /// <summary>
    /// Контактный телефон клиента
    /// </summary>
    [Required(ErrorMessage = "Контактный телефон обязателен")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Коллекция заявок, созданных данным клиентом
    /// </summary>
    public List<Request> Requests { get; set; } = new();
}
