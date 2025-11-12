namespace EstateAgency.Application.DTOs.Analytics;

/// <summary>
/// Содержит распределение заявок по категориям недвижимости
/// </summary>
public class PropertyTypeStatDto
{
    /// <summary>
    /// Тип недвижимости
    /// </summary>
    public string PropertyType { get; set; } = string.Empty;

    /// <summary>
    /// Количество заявок по данному типу
    /// </summary>
    public int RequestCount { get; set; }

    /// <summary>
    /// Процентное соотношение от общего числа заявок
    /// </summary>
    public double Percentage { get; set; }
}