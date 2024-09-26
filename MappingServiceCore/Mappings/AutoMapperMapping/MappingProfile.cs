using static MappingServiceCore.Mappings.AutoMapper.FullNameSplit;
using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using AutoMapper;

namespace MappingServiceCore.Mappings.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName}_{src.LastName}".Trim()))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate.ToLongDateString()));

            CreateMap<PersonDto, Person>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom<DtoFullNameToEntityFirstNameResolver>())
                .ForMember(dest => dest.LastName, opt => opt.MapFrom<DtoFullNameToEntityLastNameResolver>());

            CreateMap<PersonViewModel, PersonDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName}_{src.LastName}".Trim()));

            CreateMap<PersonDto, PersonViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom<DtoFullNameToViewModelFirstNameResolver>())
                .ForMember(dest => dest.LastName, opt => opt.MapFrom<DtoFullNameToViewModelLastNameResolver>());
        }
    }
}