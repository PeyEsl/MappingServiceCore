using MappingServiceCore.Services.Interfaces;
using MappingServiceCore.Services;
using MappingServiceCore.Mappings.ManualMapping;
using MappingServiceCore.Mappings.AutoMapper;
using MappingServiceCore.Mappings.ValueInjecterMapping;
using MappingServiceCore.Mappings.MapsterMapping;
using MappingServiceCore.Mappings.ExpressMapperMapping;
using MappingServiceCore.Mappings.LINQMapping;
using MappingServiceCore.Mappings.ObjectToDictionaryMapping;
using MappingServiceCore.Mappings.ObjectToJsonMapping;

namespace MappingServiceCore.Configurations.Extensions
{
    public static class DependencyInjectionServiceExtension
    {
        public static IServiceCollection AddServiceScoped(this IServiceCollection services)
        {
            // Add application services
            services.AddScoped<IMappingService, MappingService>();

            // Add Manual mapping service
            services.AddScoped<ManualConfig>();

            // Add AutoMapper services
            services.AddAutoMapper(typeof(MappingProfile));

            // Add ValueInjecter mapping service
            services.AddSingleton<ValueInjecterConfig>();

            // Add Mapster mapping service
            var mapsterConfig = MapsterConfig.GetTypeAdapterConfig();
            services.AddSingleton(mapsterConfig);
            services.AddScoped<MapsterMapping>();

            // Add ExpressMapper mapping service
            ExpressMapperConfig.RegisterMappings();

            // Add LINQ Select mapping service
            services.AddScoped<LinqSelectConfig>();

            // Add Object to Dictionary mapping service
            services.AddScoped<ObjectDictionaryReflection>();
            services.AddScoped<ObjectDictionaryJson>();
            services.AddScoped<ObjectToDictionaryLinq>();
            services.AddScoped<ObjectToDictionaryManual>();

            // Add Object to Json mapping service
            services.AddScoped<ObjectJsonNewtonsoft>();
            services.AddScoped<ObjectJsonSystemText>();

            return services;
        }
    }
}