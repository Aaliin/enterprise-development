using EstateAgency.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgency.Domain;

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
    public PropertyType Type { get; set; }

    /// <summary>
    /// Назначение объекта 
    /// </summary>
    [Required(ErrorMessage = "Назначение недвижимости обязательно")]
    public PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер объекта
    /// </summary>
    [Required(ErrorMessage = "Кадастровый номер обязателен")]
    public string CadastralNumber { get; set; } = string.Empty;

    /// <summary>
    /// Адрес расположения объекта
    /// </summary>
    [Required(ErrorMessage = "Адрес обязателен")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Общее количество этажей в здании
    /// </summary>
    [Required(ErrorMessage = "Этажность обязательна")]
    public int Floors { get; set; }

    /// <summary>
    /// Общая площадь объекта в квадратных метрах
    /// </summary>
    [Required(ErrorMessage = "Общая площадь обязательна")]
    public decimal TotalArea { get; set; }

    /// <summary>
    /// Количество комнат в объекте
    /// </summary>
    [Required(ErrorMessage = "Количество комнат обязательно")]
    public int Rooms { get; set; }

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
    public bool HasEncumbrances { get; set; }

    /// <summary>
    /// Коллекция заявок, связанных с данным объектом недвижимости
    /// </summary>
    public List<Request> Requests { get; set; } = new();
}
