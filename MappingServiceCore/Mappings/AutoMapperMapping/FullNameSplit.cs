using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using AutoMapper;

namespace MappingServiceCore.Mappings.AutoMapper
{
    public class FullNameSplit
    {
        public class DtoFullNameToEntityFirstNameResolver : IValueResolver<PersonDto, Person, string?>
        {
            public string? Resolve(PersonDto source, Person destination, string? destMember, ResolutionContext context)
            {
                return source.FullName?.Split('_')[0].Trim() ?? string.Empty;
            }
        }

        public class DtoFullNameToEntityLastNameResolver : IValueResolver<PersonDto, Person, string?>
        {
            public string? Resolve(PersonDto source, Person destination, string? destMember, ResolutionContext context)
            {
                return source.FullName?.Split('_')[1].Trim() ?? string.Empty;
            }
        }

        public class DtoFullNameToViewModelFirstNameResolver : IValueResolver<PersonDto, PersonViewModel, string?>
        {
            public string Resolve(PersonDto source, PersonViewModel destination, string? destMember, ResolutionContext context)
            {
                return source.FullName?.Split('_')[0].Trim() ?? string.Empty;
            }
        }

        public class DtoFullNameToViewModelLastNameResolver : IValueResolver<PersonDto, PersonViewModel, string?>
        {
            public string Resolve(PersonDto source, PersonViewModel destination, string? destMember, ResolutionContext context)
            {
                return source.FullName?.Split('_')[1].Trim() ?? string.Empty;
            }
        }
    }
}
