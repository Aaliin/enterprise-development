using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgency.Domain.Enum;

/// <summary>
/// Тип заявки от клиента
/// </summary>
public enum RequestType
{
    Purchase,       // Покупка недвижимости
    Sale            // Продажа недвижимости
}
