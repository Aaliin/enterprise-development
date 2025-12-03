using EstateAgency.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Application.DTOs;

/// <summary>
/// Содержит валидационные правила для технических характеристик
/// </summary>
public class CreatePropertyDto
{
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
    /// Уникальный идентификатор объекта недвижимости
    /// </summary>
    [Required(ErrorMessage = "Кадастровый номер обязателен")]
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Физический адрес расположения объекта
    /// </summary>
    [Required(ErrorMessage = "Адрес обязателен")]
    public required string Address { get; set; }

    /// <summary>
    /// Общее количество этажей в здании
    /// </summary>
    [Required(ErrorMessage = "Этажность обязательна")]
    [Range(1, 100, ErrorMessage = "Этажность должна быть от 1 до 100")]
    public int Floors { get; set; }

    /// <summary>
    /// Общая площадь объекта в квадратных метрах
    /// </summary>
    [Required(ErrorMessage = "Общая площадь обязательна")]
    [Range(0.1, 10000, ErrorMessage = "Площадь должна быть от 0.1 до 10000 м²")]
    public decimal TotalArea { get; set; }

    /// <summary>
    /// Количество комнат в объекте
    /// </summary>
    [Required(ErrorMessage = "Количество комнат обязательно")]
    [Range(0, 50, ErrorMessage = "Количество комнат должно быть от 0 до 50")]
    public int Rooms { get; set; }

    /// <summary>
    /// Высота потолков в метрах
    /// </summary>
    [Range(1, 10, ErrorMessage = "Высота потолков должна быть от 1 до 10 метров")]
    public decimal? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения объекта
    /// </summary>
    [Range(0, 100, ErrorMessage = "Этаж должен быть от 0 до 100")]
    public int? Floor { get; set; }

    /// <summary>
    /// Флаг наличия обременений
    /// </summary>
    [Required(ErrorMessage = "Наличие задолжностей обязательно")]
    public bool HasEncumbrances { get; set; }
}