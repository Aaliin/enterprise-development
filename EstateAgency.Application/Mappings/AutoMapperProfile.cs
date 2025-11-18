using AutoMapper;
using EstateAgency.Application.DTOs;
using EstateAgency.Domain.Entities; 

namespace EstateAgency.Application.Mappings;

/// <summary>
/// Для настройки преобразований между сущностями и DTO
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр 
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();

        CreateMap<Property, PropertyDto>();
        CreateMap<CreatePropertyDto, Property>();

        CreateMap<Request, RequestDto>()
            .ForMember(dest => dest.ClientFullName, opt => opt.MapFrom(src => src.Client.FullName))
            .ForMember(dest => dest.PropertyAddress, opt => opt.MapFrom(src => src.Property.Address));
        CreateMap<CreateRequestDto, Request>();
    }
}