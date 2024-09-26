using Microsoft.EntityFrameworkCore;
using MappingServiceCore.Data;

namespace MappingServiceCore.Configurations.Extensions
{
    public static class ConnectionStringExtension
    {
        public static IServiceCollection AddConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            // Get ConnectionString from AppSetting
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Configure DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add developer page exception filter
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
