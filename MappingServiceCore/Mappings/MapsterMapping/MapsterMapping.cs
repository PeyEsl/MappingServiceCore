using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using Mapster;

namespace MappingServiceCore.Mappings.MapsterMapping
{
    public class MapsterMapping
    {
        #region Ctor

        private readonly TypeAdapterConfig _config;

        public MapsterMapping(TypeAdapterConfig config)
        {
            _config = config;
        }

        #endregion

        public PersonDto MapEntityToDto(Person person)
        {
            return person.Adapt<PersonDto>(_config);
        }

        public Person MapDtoToEntity(PersonDto personDto)
        {
            return personDto.Adapt<Person>(_config);
        }

        public PersonDto MapViewModelToDto(PersonViewModel personVm)
        {
            return personVm.Adapt<PersonDto>(_config);
        }

        public PersonViewModel MapDtoToViewModel(PersonDto personDto)
        {
            return personDto.Adapt<PersonViewModel>(_config);
        }
    }
}