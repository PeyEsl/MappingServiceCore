using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;

namespace MappingServiceCore.Mappings.LINQMapping
{
    public class LinqSelectConfig
    {
        public IEnumerable<PersonDto> MapEntitiesToDtos(IEnumerable<Person> persons)
        {
            var personDtos = persons.Select(person => new PersonDto
            {
                Id = person.Id.ToString(),
                FullName = $"{person.FirstName}_{person.LastName}".Trim(),
                CreateDate = person.CreateDate.ToLongDateString(),
                PhoneNumber = person.PhoneNumber
            }).ToList();

            return personDtos;
        }

        public IEnumerable<PersonViewModel> MapDtosToViewModels(IEnumerable<PersonDto> personDtos)
        {
            var personVms = personDtos.Select(personDto => new PersonViewModel
            {
                Id = personDto.Id,
                FirstName = personDto.FullName?.Split('_')[0].Trim() ?? string.Empty,
                LastName = personDto.FullName?.Split('_').Length > 1 ? personDto.FullName?.Split('_')[1].Trim() : string.Empty,
                CreateDate = personDto.CreateDate,
                PhoneNumber = personDto.PhoneNumber
            }).ToList();

            return personVms;
        }

        public PersonDto MapEntityToDto(Person person)
        {
            return new PersonDto
            {
                Id = person.Id.ToString(),
                FullName = $"{person.FirstName}_{person.LastName}".Trim(),
                PhoneNumber = person.PhoneNumber,
                CreateDate = person.CreateDate.ToLongDateString(),
            };
        }

        public Person MapDtoToEntity(PersonDto personDto)
        {
            if (string.IsNullOrEmpty(personDto.FullName))
                throw new ArgumentException("FullName cannot be null or empty");

            var names = personDto.FullName!.Split('_');
            return new Person
            {
                Id = personDto.Id != null ? Guid.Parse(personDto.Id!) : Guid.Empty,
                FirstName = names.Length > 0 ? names[0].Trim() : string.Empty,
                LastName = names.Length > 1 ? string.Join(' ', names.Skip(1)).Trim() : string.Empty,
                PhoneNumber = personDto.PhoneNumber,
                CreateDate = personDto.CreateDate != null ? DateTime.Parse(personDto.CreateDate!) : DateTime.MinValue,
            };
        }

        public PersonDto MapViewModelToDto(PersonViewModel personVm)
        {
            return new PersonDto
            {
                Id = personVm.Id,
                FullName = $"{personVm.FirstName}_{personVm.LastName}".Trim(),
                PhoneNumber = personVm.PhoneNumber,
                CreateDate = personVm.CreateDate,
            };
        }

        public PersonViewModel MapDtoToViewModel(PersonDto personDto)
        {
            if (string.IsNullOrEmpty(personDto.FullName))
                throw new ArgumentException("FullName cannot be null or empty");

            var names = personDto.FullName!.Split('_');
            return new PersonViewModel
            {
                Id = personDto.Id,
                FirstName = names.Length > 0 ? names[0].Trim() : string.Empty,
                LastName = names.Length > 1 ? string.Join(' ', names.Skip(1)).Trim() : string.Empty,
                PhoneNumber = personDto.PhoneNumber,
                CreateDate = personDto.CreateDate,
            };
        }
    }
}
