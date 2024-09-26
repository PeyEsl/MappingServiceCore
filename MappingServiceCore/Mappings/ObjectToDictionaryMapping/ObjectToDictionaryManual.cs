using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;

namespace MappingServiceCore.Mappings.ObjectToDictionaryMapping
{
    public class ObjectToDictionaryManual
    {
        public Dictionary<string, object?> PersonToDictionary(Person person)
        {
            return new Dictionary<string, object?>
            {
                { "Id", person.Id.ToString() },
                { "FirstName", person.FirstName },
                { "LastName", person.LastName },
                { "PhoneNumber", person.PhoneNumber },
                { "CreateDate", person.CreateDate.ToString() },
            };
        }

        public Person DictionaryToPerson(Dictionary<string, object?> dict)
        {
            return new Person
            {
                Id = dict.ContainsKey("Id") ? Guid.Parse(dict["Id"]?.ToString() ?? Guid.Empty.ToString()) : Guid.Empty,
                FirstName = dict.ContainsKey("FirstName") ? dict["FirstName"]?.ToString() : string.Empty,
                LastName = dict.ContainsKey("LastName") ? dict["LastName"]?.ToString() : string.Empty,
                PhoneNumber = dict.ContainsKey("PhoneNumber") ? dict["PhoneNumber"]?.ToString() : string.Empty,
                CreateDate = dict.ContainsKey("CreateDate") ? DateTime.Parse(dict["CreateDate"]?.ToString() ?? DateTime.MinValue.ToString()) : DateTime.MinValue,
            };
        }

        public Dictionary<string, object?> PersonDtoToDictionary(PersonDto personDto)
        {
            return new Dictionary<string, object?>
            {
                { "Id", personDto.Id },
                { "FullName", personDto.FullName },
                { "PhoneNumber", personDto.PhoneNumber },
                { "CreateDate", personDto.CreateDate },
            };
        }

        public PersonDto DictionaryToPersonDto(Dictionary<string, object?> dict)
        {
            return new PersonDto
            {
                Id = dict.ContainsKey("Id") ? dict["Id"]?.ToString() : string.Empty,
                FullName = dict.ContainsKey("FullName") ? dict["FullName"]?.ToString() : string.Empty,
                PhoneNumber = dict.ContainsKey("PhoneNumber") ? dict["PhoneNumber"]?.ToString() : string.Empty,
                CreateDate = dict.ContainsKey("CreateDate") ? dict["CreateDate"]?.ToString() : string.Empty,
            };
        }

        public Dictionary<string, object?> PersonViewModelToDictionary(PersonViewModel personVm)
        {
            return new Dictionary<string, object?>
            {
                { "Id", personVm.Id },
                { "FirstName", personVm.FirstName },
                { "LastName", personVm.LastName },
                { "PhoneNumber", personVm.PhoneNumber },
                { "CreateDate", personVm.CreateDate },
            };
        }

        public PersonViewModel DictionaryToPersonViewModel(Dictionary<string, object?> dict)
        {
            return new PersonViewModel
            {
                Id = dict.ContainsKey("Id") ? dict["Id"]?.ToString() : string.Empty,
                FirstName = dict.ContainsKey("FirstName") ? dict["FirstName"]?.ToString() : string.Empty,
                LastName = dict.ContainsKey("LastName") ? dict["LastName"]?.ToString() : string.Empty,
                PhoneNumber = dict.ContainsKey("PhoneNumber") ? dict["PhoneNumber"]?.ToString() : string.Empty,
                CreateDate = dict.ContainsKey("CreateDate") ? dict["CreateDate"]?.ToString() : string.Empty,
            };
        }
    }
}
