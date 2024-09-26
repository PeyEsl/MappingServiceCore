using MappingServiceCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MappingServiceCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person>? People { get; set; }
    }
}