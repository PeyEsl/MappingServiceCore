using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using ExpressMapper;

namespace MappingServiceCore.Mappings.ExpressMapperMapping
{
    public static class ExpressMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Register<Person, PersonDto>()
                  .Member(dest => dest.Id, src => src.Id.ToString())
                  .Member(dest => dest.FullName, src => $"{src.FirstName}_{src.LastName}".Trim())
                  .Member(dest => dest.CreateDate, src => src.CreateDate.ToLongDateString());

            Mapper.Register<PersonDto, Person>()
                  .Member(dest => dest.Id, src => Guid.Parse(src.Id!))
                  .Member(dest => dest.FirstName, src => GetFirstName(src.FullName!))
                  .Member(dest => dest.LastName, src => GetLastName(src.FullName!));

            Mapper.Register<PersonViewModel, PersonDto>()
                  .Member(dest => dest.FullName, src => $"{src.FirstName}_{src.LastName}".Trim());

            Mapper.Register<PersonDto, PersonViewModel>()
                  .Member(dest => dest.FirstName, src => GetFirstName(src.FullName!))
                  .Member(dest => dest.LastName, src => GetLastName(src.FullName!));
        }

        private static string GetFirstName(string fullName)
        {
            var names = fullName.Split('_');

            return names.Length > 0 ? names[0].Trim() : string.Empty;
        }

        private static string GetLastName(string fullName)
        {
            var names = fullName.Split('_');

            return names.Length > 1 ? names[1].Trim() : string.Empty;
        }
    }
}