namespace EstateAgency.Infrastructure.Services;

/// <summary>
/// Сервис для работы с датой и временем 
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Получает текущую локальную дату и время
    /// </summary>
    public DateTime Now { get; }

    /// <summary>
    /// Получает текущую UTC дату и время
    /// </summary>
    public DateTime UtcNow { get; }
}

/// <summary>
/// Реализация сервиса даты и времени 
/// </summary>
public class DateTimeService : IDateTimeService
{
    /// <summary>
    /// Получает текущую локальную дату и время
    /// </summary>
    public DateTime Now => DateTime.Now;

    /// <summary>
    /// Получает текущую UTC дату и время
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;
}