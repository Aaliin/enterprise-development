using EstateAgency.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Domain.Entities;

/// <summary>
/// Класс, представляющий объект недвижимости
/// </summary>
public class Property
{
    /// <summary>
    /// Уникальный идентификатор объекта недвижимости
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тип объекта недвижимости 
    /// </summary>
    [Required(ErrorMessage = "Тип недвижимости обязателен")]
    public required PropertyType Type { get; set; }

    /// <summary>
    /// Назначение объекта 
    /// </summary>
    [Required(ErrorMessage = "Назначение недвижимости обязательно")]
    public required PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер объекта
    /// </summary>
    [Required(ErrorMessage = "Кадастровый номер обязателен")]
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Адрес расположения объекта
    /// </summary>
    [Required(ErrorMessage = "Адрес обязателен")]
    public required string Address { get; set; }

    /// <summary>
    /// Общее количество этажей в здании
    /// </summary>
    [Required(ErrorMessage = "Этажность обязательна")]
    public required int Floors { get; set; }

    /// <summary>
    /// Общая площадь объекта в квадратных метрах
    /// </summary>
    [Required(ErrorMessage = "Общая площадь обязательна")]
    public required decimal TotalArea { get; set; }

    /// <summary>
    /// Количество комнат в объекте
    /// </summary>
    [Required(ErrorMessage = "Количество комнат обязательно")]
    public required int Rooms { get; set; }

    /// <summary>
    /// Высота потолков в метрах
    /// </summary>
    public decimal? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж, на котором расположен объект
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Наличие задолжностей
    /// </summary>
    [Required(ErrorMessage = "Наличие задолжностей обязательно")]
    public required bool HasEncumbrances { get; set; }

    /// <summary>
    /// Коллекция заявок, связанных с данным объектом недвижимости
    /// </summary>
    public List<Request> Requests { get; set; } = [];
}
