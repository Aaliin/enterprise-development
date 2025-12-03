using EstateAgency.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Application.DTOs;

/// <summary> 
/// Содержит полные данные объекта для отображения в UI 
/// </summary>
public class PropertyDto
{
    /// <summary>
    /// Уникальный идентификатор объекта недвижимости в системе
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тип объекта недвижимости 
    /// </summary>
    public PropertyType Type { get; set; }

    /// <summary>
    /// Назначение объекта  
    /// </summary>
    public PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Кадастровый номер объекта  
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Физический адрес расположения объекта 
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Общее количество этажей в здании 
    /// </summary>
    public int Floors { get; set; }

    /// <summary>
    /// Общая площадь объекта 
    /// </summary>
    public decimal TotalArea { get; set; }

    /// <summary>
    /// Количество комнат в объекте 
    /// </summary>
    public int Rooms { get; set; }

    /// <summary>
    /// Высота потолков в метрах 
    /// </summary>
    public decimal? CeilingHeight { get; set; }

    /// <summary>
    /// Этаж расположения объекта 
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Флаг наличия обременений
    /// </summary>
    public bool HasEncumbrances { get; set; }
}