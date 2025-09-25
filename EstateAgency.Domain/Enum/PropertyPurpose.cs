using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgency.Domain.Enum;

/// <summary>
/// Назначение объекта недвижимости
/// </summary>
public enum PropertyPurpose
{
    Residential,    // Жилое 
    Commercial,     // Коммерческое 
    Industrial,     // Промышленное 
    Agricultural    // Сельскохозяйственное 
}
