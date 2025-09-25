using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgency.Domain.Enum;

/// <summary>
/// Тип объекта недвижимости
/// </summary>
public enum PropertyType
{
    Apartment,      // Квартира
    House,          // Дом
    Commercial,     // Коммерческая недвижимость
    Land,           // Земельный участок
    Villa           // Вилла
}