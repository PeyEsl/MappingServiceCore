using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using Omu.ValueInjecter;

namespace MappingServiceCore.Mappings.ValueInjecterMapping
{
    public class ValueInjecterConfig
    {
        public PersonDto MapEntityToDto(Person person)
        {
            var personDto = new PersonDto();
            personDto.InjectFrom(person);
            personDto.Id = person.Id.ToString();
            personDto.FullName = $"{person.FirstName}_{person.LastName}".Trim();
            personDto.CreateDate = person.CreateDate.ToLongDateString();

            return personDto;
        }

        public Person MapDtoToEntity(PersonDto personDto)
        {
            var person = new Person();
            person.InjectFrom(personDto);
            if (Guid.TryParse(personDto.Id, out Guid parsedId))
                person.Id = parsedId;
            else
                throw new ArgumentException("Invalid GUID format");

            var names = personDto.FullName?.Split('_');
            person.FirstName = names?[0].Trim() ?? string.Empty;
            person.LastName = names?.Length > 1 ? names[1].Trim() : string.Empty;

            return person;
        }

        public PersonDto MapViewModelToDto(PersonViewModel personVm)
        {
            var personDto = new PersonDto();
            personDto.InjectFrom(personVm);
            personDto.FullName = $"{personVm.FirstName}_{personVm.LastName}".Trim();

            return personDto;
        }

        public PersonViewModel MapDtoToViewModel(PersonDto personDto)
        {
            var personVm = new PersonViewModel();
            personVm.InjectFrom(personDto);
            var names = personDto.FullName?.Split('_');
            personVm.FirstName = names?[0].Trim() ?? string.Empty;
            personVm.LastName = names?.Length > 1 ? names[1].Trim() : string.Empty;

            return personVm;
        }
    }
}