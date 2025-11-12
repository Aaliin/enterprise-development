namespace EstateAgency.Application.Exceptions;

/// <summary> 
/// Используется для обработки случаев отсутствия запрашиваемых данных 
/// </summary>
/// <param name="entityName">Название типа сущности</param>
/// <param name="id">Идентификатор сущности, которая не была найдена</param>
public class EntityNotFoundException(string entityName, int id)
    : Exception($"{entityName} с ID {id} не найден") { }