using MappingServiceCore.Models.Entities;
using MappingServiceCore.Models.DTOs;
using MappingServiceCore.Models.ViewModels;
using Mapster;

namespace MappingServiceCore.Mappings.MapsterMapping
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig GetTypeAdapterConfig()
        {
            var config = new TypeAdapterConfig();

            config.NewConfig<Person, PersonDto>()
                  .Map(dest => dest.FullName, src => $"{src.FirstName}_{src.LastName}".Trim())
                  .Map(dest => dest.CreateDate, src => src.CreateDate.ToLongDateString());

            config.NewConfig<PersonDto, Person>()
                  .Map(dest => dest.FirstName, src => GetFirstName(src.FullName!))
                  .Map(dest => dest.LastName, src => GetLastName(src.FullName!));

            config.NewConfig<PersonViewModel, PersonDto>()
                  .Map(dest => dest.FullName, src => $"{src.FirstName}_{src.LastName}".Trim());

            config.NewConfig<PersonDto, PersonViewModel>()
                  .Map(dest => dest.FirstName, src => GetFirstName(src.FullName!))
                  .Map(dest => dest.LastName, src => GetLastName(src.FullName!));

            return config;
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